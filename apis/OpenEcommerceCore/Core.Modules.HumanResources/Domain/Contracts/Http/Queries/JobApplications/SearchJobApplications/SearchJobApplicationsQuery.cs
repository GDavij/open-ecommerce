using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.SearchJobApplications;

public record SearchJobApplicationsQuery(ApplicationProcess? ProcessStep, ECollaboratorSector? Sector, string SearchTerm = "") : IRequest<List<SearchJobApplicationsQueryResponse>>;