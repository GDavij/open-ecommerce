using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.UpdateCollaborator;
using Core.Modules.HumanResources.Domain.DtosMappings;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Exceptions.State;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.UpdateCollaborator;

internal class UpdateCollaboratorCommandHandler : IUpdateCollaboratorCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IRequestClient<GetCollaboratorIsAdminCommand> _getIsAdminRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;
    private readonly IRequestClient<GetDeletedCollaboratorsIdsCommand> _getDeletedCollaboratorsIds;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;

    public UpdateCollaboratorCommandHandler(
        IHumanResourcesContext dbContext,
        IPublishEndpoint publishEndpoint,
        IAppConfigService configService,
        ICurrentCollaboratorAsyncResolver currentCollaborator,
        IRequestClient<GetCollaboratorIsAdminCommand> getIsAdminRequestClient,
        IRequestClient<GetDeletedCollaboratorsIdsCommand> getDeletedCollaboratorsIds)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
        _currentCollaborator = currentCollaborator;
        _getIsAdminRequestClient = getIsAdminRequestClient;
        _getDeletedCollaboratorsIds = getDeletedCollaboratorsIds;
    }

    public async Task<UpdateCollaboratorCommandResponse> Handle(UpdateCollaboratorCommand request, CancellationToken cancellationToken)
    {
        Collaborator currentCollaborator = await _currentCollaborator.ResolveAsync();
        
        var deletedCollaborators = (await _getDeletedCollaboratorsIds.GetResponse<EvaluationResult<List<Guid>>>(new GetDeletedCollaboratorsIdsCommand())).Message.Eval;
        var conflictCollaborator = await _dbContext.Collaborators.FirstOrDefaultAsync(c => 
                    c.Id != request.Id &&
                    !deletedCollaborators.Contains(c.Id) &&
                    (c.Email == request.Email || c.Phone == request.Phone), cancellationToken);
        
        if (conflictCollaborator is not null)
        {
            throw new AlreadyExistentCollaboratorException(request.Email, request.Phone);
        }
        
        var existentCollaborator = await _dbContext.Collaborators
            .Include(c => c.Addresses)
            .Include(c => c.SocialLinks)
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        
        if (existentCollaborator is null)
        {
            throw new InvalidCollaboratorException(request.Id);
        }
        
        // TODO: Maybe Optimize this calls to user access module (messaging)
        bool currentCollaboratorIsAdmin = await currentCollaborator.IsAdmin(_getIsAdminRequestClient);
        bool existentCollaboratorIsAdmin = await existentCollaborator.IsAdmin(_getIsAdminRequestClient);
        
        if (!currentCollaboratorIsAdmin && existentCollaboratorIsAdmin)
        {
            throw new MissingAdministrativePrivilegesException("Update Administrator");
        }
        
        var existentCollaboratorSectors = existentCollaborator.Contracts
            .WhereValidContracts()
            .Select(c => c.Sector)
            .Distinct();
        
        if (!currentCollaboratorIsAdmin && existentCollaboratorSectors.Contains(ECollaboratorSector.HumanResources) && currentCollaborator.Id != existentCollaborator.Id)
        {
            throw new MissingAdministrativePrivilegesException("Update other Human Resources Collaborator");
        }

        var sentAddressesStates = request.Addresses
            .Select(a => a.StateId)
            .Distinct()
            .ToList();
        
        var validAddressesStates = await _dbContext.States
            .Where(s => sentAddressesStates.Contains(s.Id))
            .ToListAsync(cancellationToken);

        if (validAddressesStates.Count < sentAddressesStates.Count)
        {
            var validAddressesStatesIds = validAddressesStates.Select(v => v.Id);
            var firstInvalidState = sentAddressesStates.First(s => !validAddressesStatesIds.Contains(s));
            
            throw new InvalidStateException(firstInvalidState);
        }
        
        bool shouldPublishUpdatedCollaboratorEvent = request.Password is not null || existentCollaborator.Email != request.Email;

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        existentCollaborator.FirstName = request.FirstName;
        existentCollaborator.LastName = request.LastName;
        existentCollaborator.Email = request.Email;
        existentCollaborator.Description = request.Description;
        existentCollaborator.Age = request.Age;
        existentCollaborator.Phone = request.Phone;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _dbContext.SocialLinks.RemoveRange(existentCollaborator.SocialLinks);

         var updatedSocialLinks = request.SocialLinks.Select(s => new SocialLink
         {
             Id = Guid.NewGuid(),
             Collaborator = existentCollaborator,
             SocialMedia = s.SocialMedia,
             URL = s.Url
         }).ToList();
       
        _dbContext.SocialLinks.AddRange(updatedSocialLinks);

        _dbContext.Addresses.RemoveRange(existentCollaborator.Addresses);
        
        var updatedAddresses = request.Addresses.Select(a => new Address
        {
            Id = Guid.NewGuid(),
            Collaborator = existentCollaborator,
            Neighbourhood = a.Neighbourhood,
            State = validAddressesStates.First(va => va.Id == a.StateId),
            Street = a.Street,
            ZipCode = a.ZipCode
        }).ToList();
        
        _dbContext.Addresses.AddRange(updatedAddresses);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        
        if (shouldPublishUpdatedCollaboratorEvent)
        {
            //TODO: Add Retry with Polly
            await _publishEndpoint.Publish(UpdatedCollaboratorIntegrationEvent.CreateEvent(existentCollaborator.MapToUpdatedDto(request.Password)));
        }
        
        return UpdateCollaboratorCommandResponse.Respond(existentCollaborator.Id, _configService);
    }
}