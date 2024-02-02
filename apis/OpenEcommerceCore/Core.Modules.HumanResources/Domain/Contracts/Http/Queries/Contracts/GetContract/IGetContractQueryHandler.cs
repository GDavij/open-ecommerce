using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.GetContract;

internal interface IGetContractQueryHandler
    : IRequestHandler<GetContractQuery, GetContractQueryResponse>
{ }