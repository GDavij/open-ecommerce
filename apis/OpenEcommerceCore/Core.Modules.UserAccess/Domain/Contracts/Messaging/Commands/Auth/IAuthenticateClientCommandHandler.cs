using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands.Auth;

public interface IAuthenticateClientCommandHandler
    : IConsumer<AuthenticateClientCommand>
{ }