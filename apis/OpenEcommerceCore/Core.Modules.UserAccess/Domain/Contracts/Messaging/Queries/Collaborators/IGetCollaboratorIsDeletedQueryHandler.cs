using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Collaborators;

internal interface IGetCollaboratorIsDeletedQueryHandler
    : IConsumer<GetCollaboratorIsDeletedQuery>
{ }