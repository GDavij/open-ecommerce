using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.CreateCollaborator;
using Core.Modules.HumanResources.Domain.DtosMappings;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Exceptions.State;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.Collaborators.CreateCollaborator;

internal class CreateCollaboratorCommandHandler : ICreateCollaboratorCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;
    private readonly IRequestClient<GetDeletedCollaboratorsIdsCommandQuery> _getDeletedCollaboratorsIdsClient;
    
    public CreateCollaboratorCommandHandler(IHumanResourcesContext dbContext, IPublishEndpoint publishEndpoint, IAppConfigService configService, IRequestClient<GetDeletedCollaboratorsIdsCommandQuery> getDeletedCollaboratorsIdsClient)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
        _getDeletedCollaboratorsIdsClient = getDeletedCollaboratorsIdsClient;
    }

    public async Task<CreateCollaboratorCommandResponse> Handle(CreateCollaboratorCommand request, CancellationToken cancellationToken)
    {
        var deletedCollaborators = (await _getDeletedCollaboratorsIdsClient.GetResponse<EvaluationResult<HashSet<Guid>>>(new GetDeletedCollaboratorsIdsCommandQuery())).Message.Eval;       
        
        var existentCollaborator = await _dbContext.Collaborators.FirstOrDefaultAsync(c => !deletedCollaborators.Contains(c.Id) && (c.Email == request.Email || c.Phone == request.Phone), cancellationToken);
        if (existentCollaborator is not null)
        {
            throw new AlreadyExistentCollaboratorException(request.Email, request.Phone);
        }

        var addressesSent = request.Addresses.Select(a => a.StateId).Distinct().ToList();
        var validAddressesStates = await _dbContext.States.Where(s => addressesSent.Contains(s.Id)).ToListAsync(cancellationToken);

        if (validAddressesStates.Count < addressesSent.Count)
        {
            var validGuids = validAddressesStates.Select(va => va.Id);
            var firstInvalidId = addressesSent.First(a => !validGuids.Contains(a));
            
            throw new InvalidStateException(firstInvalidId);
        }

        var collaborator = new Collaborator
        {
            Id = Guid.NewGuid(),
            Age = request.Age,
            Email = request.Email,
            Description = request.Description,
            Phone = request.Phone,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Addresses = request.Addresses.Select(a => new Address
            {
                Id = Guid.NewGuid(),
                State = validAddressesStates.First(va => va.Id == a.StateId),
                Neighbourhood = a.Neighbourhood,
                Street = a.Street,
                ZipCode = a.ZipCode
            }).ToList(),
            Contracts = request.Contracts.Select(c => new Contract
            {
                Id = Guid.NewGuid(),
                Name = c.Name,
                Sector = c.Sector,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                MonthlySalary = c.MonthlySalary,
                Broken = c.Broken,
                ContributionYears = c.ContributionsYears.Select(cy => new ContributionYear
                {
                    Id = Guid.NewGuid(),
                    Year = cy.Year,
                    WorkHours = cy.WorkHours.Select(w => new WorkHour
                    {
                        Id = Guid.NewGuid(),
                        Date = w.Date,
                        Start = w.Start,
                        End = w.End,
                    }).ToList()
                }).ToList()
            }).ToList()
        };

        _dbContext.Collaborators.Add(collaborator);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //Add Retry with Polly
        await _publishEndpoint.Publish(CreatedCollaboratorIntegrationEvent.CreateEvent(collaborator.MapToCreatedDto(request.Password, false)));

        return CreateCollaboratorCommandResponse.Respond(_configService);
    }
}