using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.IntegrationEvents.Dtos;

public record CollaboratorCreatedDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public List<ECollaboratorSector> Sectors { get; init; }
    public bool Deleted { get; init; }
};

internal static class CollaboratorDtoMapping
{
    public static CollaboratorCreatedDto MapToCreatedDto(this Collaborator collaborator, string password)
    {
        return new CollaboratorCreatedDto
        {
            Id = collaborator.Id,
            FirstName = collaborator.FirstName,
            LastName = collaborator.LastName,
            Password = password,
            Email = collaborator.Email,
            Sectors = collaborator.Contracts.Select(c => c.Sector).Distinct().ToList(),
            Deleted = collaborator.Deleted
        };
    }
}