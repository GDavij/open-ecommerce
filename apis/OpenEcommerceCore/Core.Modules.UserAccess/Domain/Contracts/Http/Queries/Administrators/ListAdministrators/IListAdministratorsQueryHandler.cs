using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;

internal interface IListAdministratorsQueryHandler
    : IRequestHandler<ListAdministratorsQuery, SearchResult<PaginatedList<ListAdministratorsQueryResponse>>>
{ }