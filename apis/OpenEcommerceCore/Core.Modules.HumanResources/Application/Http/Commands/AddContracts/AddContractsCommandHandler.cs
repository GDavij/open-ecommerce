using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.AddContracts;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Exceptions.Contracts;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.AddContracts;

internal class AddContractsCommandHandler : IAddContractsCommandHandler
{
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    private readonly IRequestClient<GetCollaboratorIsAdminCommand> _getCollaboratorIsAdminClient;
    private readonly IHumanResourcesContext _dbContext;
    private readonly IRequestClient<GetDeletedCollaboratorsIdsCommand> _getDeletedCollaboratorsClient;
    private readonly IAppConfigService _configService;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public AddContractsCommandHandler(
        ICurrentCollaboratorAsyncResolver currentCollaborator,
        IRequestClient<GetCollaboratorIsAdminCommand> getCollaboratorIsAdminClient,
        IHumanResourcesContext dbContext,
        IRequestClient<GetDeletedCollaboratorsIdsCommand> getDeletedCollaboratorsClient, IAppConfigService configService, IPublishEndpoint publishEndpoint)
    {
        _currentCollaborator = currentCollaborator;
        _getCollaboratorIsAdminClient = getCollaboratorIsAdminClient;
        _dbContext = dbContext;
        _getDeletedCollaboratorsClient = getDeletedCollaboratorsClient;
        _configService = configService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<AddContractsCommandResponse> Handle(AddContractsCommand request, CancellationToken cancellationToken)
    {
        var deletedCollaborators = (await _getDeletedCollaboratorsClient.GetResponse<EvaluationResult<List<Guid>>>(new GetDeletedCollaboratorsIdsCommand(), cancellationToken)).Message.Eval;
        
        var existentCollaborator = await _dbContext.Collaborators
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => !deletedCollaborators.Contains(c.Id) && c.Id == request.CollaboratorId, cancellationToken);

        if (existentCollaborator is null)
        {
            throw new InvalidCollaboratorException(request.CollaboratorId);
        }
        
        var currentCollaborator = await _currentCollaborator.ResolveAsync();
        if (existentCollaborator.Id == currentCollaborator.Id)
        {
            throw new CollaboratorCannotUpdateItsContractsItSelfException();
        }
        
        var existentCollaboratorIsAdmin = await existentCollaborator.IsAdmin(_getCollaboratorIsAdminClient);
        if (existentCollaboratorIsAdmin)
        {
            throw new AdminCannotHaveContractsException();
        }
       
        var existentCollaboratorSectors = existentCollaborator.Contracts
            .WhereValidContracts()
            .Select(c => c.Sector)
            .ToHashSet();
        
        var currentCollaboratorIsAdmin = await currentCollaborator.IsAdmin(_getCollaboratorIsAdminClient);
        if (!currentCollaboratorIsAdmin && existentCollaboratorSectors.Contains(ECollaboratorSector.HumanResources))
        {
            throw new MissingAdministrativePrivilegesException("Update Human Resources Collaborator Contracts");
        }
        
        var requestSectors = request.Contracts
            .Select(c => c.Sector)
            .ToHashSet();
        
        requestSectors.IntersectWith(existentCollaboratorSectors);
        if (requestSectors.Count > 0)
        {
            var firstInvalidSector = requestSectors.First();
            throw new CollaboratorAlreadyHaveContractForSectorException(firstInvalidSector);
        }

        var addedContracts = request.Contracts.Select(c => new Contract
        {
            Id = Guid.NewGuid(),
            Name = c.Name,
            Sector = c.Sector,
            ContributionYears = c.ContributionsYears.Select(cy => new ContributionYear
            {
                Id = Guid.NewGuid(),
                WorkHours = cy.WorkHours.Select(w => new WorkHour
                {
                    Id = Guid.NewGuid(),
                    Date = w.Date,
                    Start = w.Start,
                    End = w.End
                }).ToList(),
                Year = cy.Year,
            }).ToList(),
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            MonthlySalary = c.MonthlySalary,
            Broken = c.Broken,
        }).ToList();
        
        existentCollaborator.Contracts.AddRange(addedContracts);

        foreach (var addedContract in addedContracts)
        {
            _dbContext.Entry(addedContract).State = EntityState.Added;
            
            foreach (var addedContributionYear in addedContract.ContributionYears)
            {
                _dbContext.Entry(addedContributionYear).State = EntityState.Added;

                foreach (var addedWorkHour in addedContributionYear.WorkHours)
                {
                    _dbContext.Entry(addedWorkHour).State = EntityState.Added;
                }
            }
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        var addedValidSectors = request.Contracts
            .Where(c => !c.Broken && c.EndDate > DateTime.UtcNow)
            .Select(c => c.Sector)
            .ToList();

        //TODO: Add Retry With Polly
        await _publishEndpoint.Publish(AddedContractsIntegrationEvent.CreateEvent(existentCollaborator.Id, addedValidSectors));
        
        return AddContractsCommandResponse.Respond(existentCollaborator.Id, _configService);
    }
}