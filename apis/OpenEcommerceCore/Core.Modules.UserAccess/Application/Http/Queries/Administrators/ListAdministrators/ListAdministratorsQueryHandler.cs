using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.Extensions;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Queries.Administrators.ListAdministrators;

internal class ListAdministratorsQueryHandler : IListAdministratorsQueryHandler
{
    private readonly IUserAccessContext _dbContext;
    private const int RowsPerPage = 50;
    
    public ListAdministratorsQueryHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SearchResult<PaginatedList<ListAdministratorsQueryResponse>>> Handle(ListAdministratorsQuery request, CancellationToken cancellationToken)
    {
        var administratorsQueryable = _dbContext.Collaborators
            .Where(c => c.IsAdmin)
            .WhereIf(request.IsDeleted != null, c => c.Deleted == request.IsDeleted)
            .WhereIf(!string.IsNullOrWhiteSpace(request.Email), c => EF.Functions.ILike("%" + c.Email + "%", $"%{request.Email}%"))
            .WhereIf(request.Period != null, c => c.CreatedAt.Date >= request.Period!.Start.Date && c.CreatedAt.Date <= request.Period.End.Date);

        var foundAdministrators = administratorsQueryable
            .Select(c => new ListAdministratorsQueryResponse 
            { 
                Id = c.Id, 
                Email = c.Email, 
                CreatedAt = c.CreatedAt, 
                Deleted = c.Deleted 
            })
            .OrderBy(c => c.Email);
        
        var administrators = await foundAdministrators.ToPaginatedListAsync(RowsPerPage, request.Page, cancellationToken);
            
        var result = SearchResult<PaginatedList<ListAdministratorsQueryResponse>>.SearchFound(administrators);
        return result;
    }
}