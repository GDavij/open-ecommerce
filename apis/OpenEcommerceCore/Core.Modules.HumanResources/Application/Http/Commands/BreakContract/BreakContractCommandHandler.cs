using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.HumanResources.Domain.Exceptions.Contracts;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.BreakContract;

internal class BreakContractCommandHandler : IBreakContractCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    private readonly IRequestClient<GetCollaboratorIsAdminCommand> _getCollaboratorIsAdminClient;
    
    public BreakContractCommandHandler(
        IHumanResourcesContext dbContext,
        ICurrentCollaboratorAsyncResolver currentCollaborator,
        IRequestClient<GetCollaboratorIsAdminCommand> getCollaboratorIsAdminClient)
    {
        _dbContext = dbContext;
        _currentCollaborator = currentCollaborator;
        _getCollaboratorIsAdminClient = getCollaboratorIsAdminClient;
    }

    public async Task Handle(BreakContractCommand request, CancellationToken cancellationToken)
    {
        var currentCollaborator = await _currentCollaborator.ResolveAsync();
        
        var existentContract = await _dbContext.Contracts
            .Include(c => c.Collaborator)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existentContract is null)
        {
            throw new InvalidContractException(request.Id);
        }

        if (existentContract.Broken)
        {
            throw new AlreadyBrokenContractException(existentContract.Id);
        }

        var currentCollaboratorIsAdmin = await currentCollaborator.IsAdmin(_getCollaboratorIsAdminClient);
        
        var existentCollaborator = existentContract.Collaborator;
        var existentCollaboratorSectors = existentCollaborator.Contracts
            .WhereValidContracts()
            .Select(c => c.Sector)
            .Distinct();
        
        if (!currentCollaboratorIsAdmin && existentCollaboratorSectors.Contains(ECollaboratorSector.HumanResources))
        {
            throw new MissingAdministrativePrivilegesException("Break Human Resources Collaborator Contract");
        }

        existentContract.Broken = true;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}