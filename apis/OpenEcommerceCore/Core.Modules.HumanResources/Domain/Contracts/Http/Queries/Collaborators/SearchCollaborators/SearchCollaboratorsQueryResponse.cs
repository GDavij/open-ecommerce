namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.SearchCollaborators;

public record SearchCollaboratorsQueryResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; }
    public string Description { get; init; }
    public string Email { get; init; }
    public int Age { get; init; }
    public string Phone { get; init; }
    public bool Deleted { get; init; }
}