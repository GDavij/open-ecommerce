using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.GetContract;

public record GetContractQuery(Guid Id) : IRequest<GetContractQueryResponse>;