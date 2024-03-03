using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;

public record ListAdministratorsQuery(int Page, bool? IsDeleted, string? Email, Period? Period) : IRequest<SearchResult<PaginatedList<ListAdministratorsQueryResponse>>>;