using Core.Modules.HumanResources.Domain.Entities;

namespace Core.Modules.HumanResources.Domain.Extensions;

internal static class ContractsExtensions
{
    public static IEnumerable<Contract> WhereValidContracts(this IEnumerable<Contract> contracts)
    {
        return contracts.Where(c =>
                                !c.Deleted &&
                                !c.Broken &&
                                DateTime.UtcNow < c.EndDate);
    }
    
    public static IQueryable<Contract> WhereValidContracts(this IQueryable<Contract> contracts)
    {
        return contracts.Where(c =>
                                !c.Deleted &&
                                !c.Broken &&
                                DateTime.UtcNow < c.EndDate);
    }
}