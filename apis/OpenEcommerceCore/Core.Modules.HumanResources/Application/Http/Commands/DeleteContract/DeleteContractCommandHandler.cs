using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteContract;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Exceptions.Contracts;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.DeleteContract;

internal class DeleteContractCommandHandler : IDeleteContractCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    private readonly IRequestClient<GetCollaboratorIsAdminCommand> _getCollaboratorIsAdminClient;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public DeleteContractCommandHandler(IHumanResourcesContext dbContext, ICurrentCollaboratorAsyncResolver currentCollaborator, IRequestClient<GetCollaboratorIsAdminCommand> getCollaboratorIsAdminClient, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _currentCollaborator = currentCollaborator;
        _getCollaboratorIsAdminClient = getCollaboratorIsAdminClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DeleteContractCommand request, CancellationToken cancellationToken)
    {
        var existentContract = await _dbContext.Contracts
            .Include(c => c.Collaborator)
                .ThenInclude(c => c.Contracts)
            .WhereValidContracts()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existentContract is null)
        {
            throw new InvalidContractException(request.Id);
        }
        
        var existentCollaborator = existentContract.Collaborator;
        var existentCollaboratorIsAdmin = await existentCollaborator.IsAdmin(_getCollaboratorIsAdminClient);
        if (existentCollaboratorIsAdmin)
        {
            throw new AdminCannotHaveContractsException();
        }
        
        var currentCollaborator = await _currentCollaborator.ResolveAsync(); 
        var currentCollaboratorIsAdmin = await currentCollaborator.IsAdmin(_getCollaboratorIsAdminClient);

        var existentCollaboratorSectors = existentCollaborator.Contracts
            .WhereValidContracts()
            .Select(c => c.Sector)
            .ToHashSet();

        if (!currentCollaboratorIsAdmin && existentCollaboratorSectors.Contains(ECollaboratorSector.HumanResources))
        {
            throw new MissingAdministrativePrivilegesException("Delete human resources collaborator contract");
        }

        bool shouldDeleteCollaborator = existentCollaboratorSectors.Count == 1; 

        existentContract.Deleted = true;
        existentContract.Broken = true;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _publishEndpoint.Publish(BrokeContractIntegrationEvent.CreateEvent(existentCollaborator.Id, existentContract.Sector));

        if (shouldDeleteCollaborator)
        {
            await _publishEndpoint.Publish(DeletedCollaboratorIntegrationEvent.CreateEvent(existentCollaborator.Id));
        }
    }
}