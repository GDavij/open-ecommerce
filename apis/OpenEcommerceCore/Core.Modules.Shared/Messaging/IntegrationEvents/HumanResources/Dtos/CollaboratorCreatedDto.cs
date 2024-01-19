using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Dtos;

public record CollaboratorCreatedDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public List<ECollaboratorSector> Sectors { get; init; }
    public bool IsAdmin { get; init; }
    public bool Deleted { get; init; }
};
