namespace Core.Modules.Stock.Application.IntegrationEvents;

public abstract class BaseIntegrationEvent
{
    public Guid IntegrationEventId { get; } = Guid.NewGuid();
}