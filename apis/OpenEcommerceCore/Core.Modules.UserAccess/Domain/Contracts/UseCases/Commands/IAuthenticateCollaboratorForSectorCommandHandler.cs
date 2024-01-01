using Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateCollaboratorForSector;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface IAuthenticateCollaboratorForSectorCommandHandler
    : IConsumer<AuthenticateCollaboratorForSectorCommand>
{ }