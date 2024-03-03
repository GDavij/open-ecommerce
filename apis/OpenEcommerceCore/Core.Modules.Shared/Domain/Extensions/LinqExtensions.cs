using System.Linq.Expressions;

namespace Core.Modules.Shared.Domain.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, bool executeCondition, Expression<Func<T, bool>> whereCondition)
    {
        if (executeCondition)
        {
            return queryable.Where(whereCondition);
        }

        return queryable;
    }
}