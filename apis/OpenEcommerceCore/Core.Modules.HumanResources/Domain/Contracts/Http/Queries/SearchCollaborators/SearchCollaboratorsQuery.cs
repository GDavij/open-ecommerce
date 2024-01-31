using Core.Modules.Shared.Domain.BusinessHierarchy;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchCollaborators;

public record SearchCollaboratorsQuery(ECollaboratorSector? Sector, bool? IsDeleted, string SearchTerm = "") : IRequest<List<SearchCollaboratorsQueryResponse>>;