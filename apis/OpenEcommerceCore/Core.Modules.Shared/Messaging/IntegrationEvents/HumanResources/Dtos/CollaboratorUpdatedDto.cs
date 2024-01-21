namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Dtos;

public class CollaboratorUpdatedDto 
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string? Password { get; init; }
}