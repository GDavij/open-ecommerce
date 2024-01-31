using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Administrators;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Administrators;

internal interface IGetAdministratorsIdsQueryHandler
    : IConsumer<GetAdministratorsIdsQuery>
{ }