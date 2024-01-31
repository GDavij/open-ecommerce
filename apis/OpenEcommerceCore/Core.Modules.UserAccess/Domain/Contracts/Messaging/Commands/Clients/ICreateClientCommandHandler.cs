using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands.Clients;

public interface ICreateClientCommandHandler 
    : IConsumer<CreateClientCommand>
{ }