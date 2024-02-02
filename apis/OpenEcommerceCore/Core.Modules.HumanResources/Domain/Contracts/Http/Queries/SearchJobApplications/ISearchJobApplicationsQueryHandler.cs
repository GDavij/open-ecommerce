using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchJobApplication;

internal interface ISearchJobApplicationsQueryHandler
    : IRequestHandler<SearchJobApplicationsQuery, List<SearchJobApplicationsQueryResponse>>
{ }