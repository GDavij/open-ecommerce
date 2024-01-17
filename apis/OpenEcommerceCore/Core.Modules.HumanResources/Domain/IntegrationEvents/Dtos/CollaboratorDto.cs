using Core.Modules.HumanResources.Domain.Entities;

namespace Core.Modules.HumanResources.Domain.IntegrationEvents.Dtos;

public record CollaboratorDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; }
    public bool Deleted { get; init; }
};

internal static class CollaboratorDtoMapping
{
    public static CollaboratorDto MapToDto(this Collaborator collaborator)
    {
        return new CollaboratorDto
        {
            Id = collaborator.Id,
            FirstName = collaborator.FirstName,
            LastName = collaborator.LastName,
            Email = collaborator.Email,
            Deleted = collaborator.Deleted
        };
    }
}