using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;

public interface ICreateClientCommandHandler 
    : IConsumer<CreateClientCommand>
{ }