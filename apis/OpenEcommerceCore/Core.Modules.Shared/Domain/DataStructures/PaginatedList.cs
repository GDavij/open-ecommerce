namespace Core.Modules.Shared.Domain.DataStructures;

public class PaginatedList<T>
{
    public List<T> Page { get; init; }
    public int MaxPages { get; init; }
    public int pageIndex { get; init; } = 1;
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }
}