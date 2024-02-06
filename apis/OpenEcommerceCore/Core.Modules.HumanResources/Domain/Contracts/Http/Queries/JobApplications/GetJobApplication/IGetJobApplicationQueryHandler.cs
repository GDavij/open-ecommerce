using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.GetJobApplication;

internal interface IGetJobApplicationQueryHandler 
    : IRequestHandler<GetJobApplicationQuery, GetJobApplicationQueryResponse>
{ }