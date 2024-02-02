using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.SearchJobApplications;

internal interface ISearchJobApplicationsQueryHandler
    : IRequestHandler<SearchJobApplicationsQuery, List<SearchJobApplicationsQueryResponse>>
{ }