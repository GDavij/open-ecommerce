using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.DataStructures;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.SearchContracts;

public record SearchContractsQuery(ECollaboratorSector? Sector, DateTime? FromDate, DateTime? TillDate, bool? IsBroken, bool? IsDeleted, bool? IsExpired, int Page, string SearchTerm  = "") : IRequest<PaginatedList<SearchContractsQueryResponse>>;