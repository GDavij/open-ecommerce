namespace Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;

public record ListAdministratorsQueryResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public DateTime CreatedAt { get; init; }
    public bool Deleted { get; init; }
}