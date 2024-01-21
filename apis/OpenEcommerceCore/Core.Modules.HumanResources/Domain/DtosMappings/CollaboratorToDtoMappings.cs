using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Dtos;

namespace Core.Modules.HumanResources.Domain.DtosMappings;

internal static class CollaboratorToDtoMapping
{
    public static CollaboratorCreatedDto MapToCreatedDto(this Collaborator collaborator, string password, bool isAdmin)
    {
        return new CollaboratorCreatedDto
        {
            Id = collaborator.Id,
            FirstName = collaborator.FirstName,
            LastName = collaborator.LastName,
            Password = password,
            Email = collaborator.Email,
            Sectors = collaborator.Contracts.Select(c => c.Sector).Distinct().ToList(),
            IsAdmin = isAdmin,
        };
    }
    
    public static CollaboratorUpdatedDto MapToUpdatedDto(this Collaborator collaborator, string? password)
    {
        return new CollaboratorUpdatedDto
        {
            Id = collaborator.Id,
            Email = collaborator.Email,
            Password = password
        };
    }
}
