namespace Core.Modules.Stock.Domain.IntegrationEvents;

public abstract class BaseIntegrationEvent
{
    public Guid IntegrationEventId { get; } = Guid.NewGuid();
}