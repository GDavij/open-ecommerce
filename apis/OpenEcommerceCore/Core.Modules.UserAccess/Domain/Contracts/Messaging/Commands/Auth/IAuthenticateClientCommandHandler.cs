using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Commands.UserAccess.Auth;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands.Auth;

public interface IAuthenticateClientCommandHandler
    : IConsumer<AuthenticateClientCommand>
{ }