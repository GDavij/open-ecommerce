using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Collaborators;

internal interface IGetCollaboratorIsAdminQueryHandler
    : IConsumer<GetCollaboratorIsAdminCommand>
{ }