namespace Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;

public record DeletedCollaboratorIntegrationEvent
{
    public Guid Id { get; init; }
    
    public static DeletedCollaboratorIntegrationEvent CreateEvent(Guid id)
    {
        return new DeletedCollaboratorIntegrationEvent
        {
            Id = id
        };
    }
};