using Core.Modules.Shared.Domain.DataStructures;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Shared.Domain.Extensions;

public static class PaginatedListExtensions
{ 
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> queryable, int rowsPerPage, int pageIndex, CancellationToken cancellationToken)
    {
        int numberOfRowsToSkip = (pageIndex - 1) * rowsPerPage;
        
        var rows = await queryable
            .Skip(numberOfRowsToSkip)
            .Take(rowsPerPage)
            .ToListAsync(cancellationToken);
        
        int maxPages = (rows.Count + numberOfRowsToSkip + rowsPerPage - 1) / rowsPerPage;
        return new PaginatedList<T>
        {
            Page = rows,
                pageIndex = pageIndex,
                MaxPages = maxPages,
                HasNextPage = pageIndex < maxPages,
                HasPreviousPage = pageIndex > 1
        };
    }
}