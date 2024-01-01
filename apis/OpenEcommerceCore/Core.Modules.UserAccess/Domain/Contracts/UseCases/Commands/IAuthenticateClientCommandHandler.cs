using Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateClient;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface IAuthenticateClientCommandHandler
    : IConsumer<AuthenticateClientCommand>
{ }