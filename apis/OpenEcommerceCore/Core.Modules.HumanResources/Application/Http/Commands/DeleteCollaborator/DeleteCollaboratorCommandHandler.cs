using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteCollaborator;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.DeleteCollaborator;

internal class DeleteCollaboratorCommandHandler : IDeleteCollaboratorCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    private readonly IRequestClient<GetCollaboratorIsAdminQuery> _getIsAdminClient;
    private readonly IRequestClient<GetCollaboratorIsDeletedQuery> _getIsDeletedClient;
    
    public DeleteCollaboratorCommandHandler(
        IHumanResourcesContext dbContext,
        IPublishEndpoint publishEndpoint,
        ICurrentCollaboratorAsyncResolver currentCollaborator,
        IRequestClient<GetCollaboratorIsAdminQuery> getIsAdminClient,
        IRequestClient<GetCollaboratorIsDeletedQuery> getIsDeletedClient)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _currentCollaborator = currentCollaborator;
        _getIsAdminClient = getIsAdminClient;
        _getIsDeletedClient = getIsDeletedClient;
    }

    public async Task Handle(DeleteCollaboratorCommand request, CancellationToken cancellationToken)
    {
        var currentCollaborator = await _currentCollaborator.ResolveAsync();

        var existentCollaborator = await _dbContext.Collaborators
            .Include(c => c.Contracts)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (existentCollaborator is null)
        {
            throw new InvalidCollaboratorException(request.Id);
        }

        if (await existentCollaborator.IsDeleted(_getIsDeletedClient))
        {
            throw new AlreadyDeletedCollaboratorException(existentCollaborator.Id);
        }

        var currentCollaboratorIsAdmin = await currentCollaborator.IsAdmin(_getIsAdminClient);
        var existentCollaboratorIsAdmin = await existentCollaborator.IsAdmin(_getIsAdminClient);

        if (!currentCollaboratorIsAdmin && existentCollaboratorIsAdmin)
        {
            throw new MissingAdministrativePrivilegesException("Delete a Administrator");
        }

        var existentCollaboratorSectors = existentCollaborator.Contracts
            .WhereValidContracts()
            .Select(c => c.Sector)
            .Distinct();
        
        if (!currentCollaboratorIsAdmin && existentCollaboratorSectors.Contains(ECollaboratorSector.HumanResources))
        {
            throw new MissingAdministrativePrivilegesException("Delete a Human Resources Collaborator");
        }

        foreach (var contract in existentCollaborator.Contracts)
        {
            contract.Broken = true;
            contract.Deleted = true;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: Add Retry with Polly
        await _publishEndpoint.Publish(DeletedCollaboratorIntegrationEvent.CreateEvent(existentCollaborator.Id));
    }
}