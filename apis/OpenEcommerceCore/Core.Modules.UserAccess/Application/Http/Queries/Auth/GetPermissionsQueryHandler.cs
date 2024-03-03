using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Auth.GetPermissions;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Queries.Auth;

internal class GetPermissionsQueryHandler : IGetPermissionsQueryHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    
    public GetPermissionsQueryHandler(IUserAccessContext dbContext, ICurrentCollaboratorAsyncResolver currentCollaborator)
    {
        _dbContext = dbContext;
        _currentCollaborator = currentCollaborator;
    }

    public async Task<EvaluationResult<IEnumerable<string>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var collaborator = await _currentCollaborator.ResolveAsync();
        
        if (collaborator.IsAdmin)
        {
            var allPermissions = new List<ECollaboratorSector>
                {
                    ECollaboratorSector.Stock,
                    ECollaboratorSector.HumanResources 
                };
            
            return new EvaluationResult<IEnumerable<string>>(allPermissions.Select(s => $"{s:G}"));
        }
        
        return new EvaluationResult<IEnumerable<string>>(collaborator.Sectors.Select(s => $"{s:G}"));
    }
}