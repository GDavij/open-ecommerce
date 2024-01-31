using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands.Collaborators;

internal interface IRemoveCollaboratorSectorCommandHandler
    : IConsumer<BrokeContractIntegrationEvent>
{ }