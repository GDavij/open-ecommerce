using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Exceptions.Contracts;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.BreakContract;

internal class BreakContractCommandHandler : IBreakContractCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    private readonly IRequestClient<GetCollaboratorIsAdminQuery> _getCollaboratorIsAdminClient;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public BreakContractCommandHandler(
        IHumanResourcesContext dbContext,
        ICurrentCollaboratorAsyncResolver currentCollaborator,
        IRequestClient<GetCollaboratorIsAdminQuery> getCollaboratorIsAdminClient,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _currentCollaborator = currentCollaborator;
        _getCollaboratorIsAdminClient = getCollaboratorIsAdminClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(BreakContractCommand request, CancellationToken cancellationToken)
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

        var numberOfCollaboratorContracts = existentCollaborator.Contracts
            .WhereValidContracts()
            .Count();
        
        if (numberOfCollaboratorContracts == 1)
        {
            throw new CannotBreakUniqueContractException(existentCollaborator.Id);
        }
        
        var currentCollaborator = await _currentCollaborator.ResolveAsync();
        var currentCollaboratorIsAdmin = await currentCollaborator.IsAdmin(_getCollaboratorIsAdminClient);

        var existentCollaboratorSectors = existentCollaborator.Contracts
            .WhereValidContracts()
            .Select(c => c.Sector)
            .ToHashSet();
        
        if (!currentCollaboratorIsAdmin && existentCollaboratorSectors.Contains(ECollaboratorSector.HumanResources))
        {
            throw new MissingAdministrativePrivilegesException("Break Human Resources Collaborator Contract");
        }

        existentContract.Broken = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: Add Retry with Polly
        await _publishEndpoint.Publish(BrokeContractIntegrationEvent.CreateEvent(existentCollaborator.Id, existentContract.Sector));
    }
}