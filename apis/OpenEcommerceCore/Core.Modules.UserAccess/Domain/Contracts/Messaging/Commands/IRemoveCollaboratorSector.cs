using Core.Modules.Shared.Messaging.IntegrationEvents.HumanResources.Events.Contracts;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;

internal interface IRemoveCollaboratorSector
    : IConsumer<BrokeContractIntegrationEvent>
{ }