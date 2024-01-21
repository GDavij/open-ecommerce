using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;

internal interface IGetCollaboratorIsAdminCommandHandler
    : IConsumer<GetCollaboratorIsAdminCommand>
{ }