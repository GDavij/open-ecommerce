using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Collaborators;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands.Collaborators;

public interface ICreateCollaboratorCommandHandler
    : IConsumer<CreatedCollaboratorIntegrationEvent>
{ }