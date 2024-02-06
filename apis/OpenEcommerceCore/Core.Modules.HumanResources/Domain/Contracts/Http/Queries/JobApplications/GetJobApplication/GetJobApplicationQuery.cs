using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.GetJobApplication;

public record GetJobApplicationQuery(Guid Id) : IRequest<GetJobApplicationQueryResponse>;