using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProductTag;

internal interface IGetProductTagQueryHandler
    : IRequestHandler<GetProductTagQuery, GetProductTagQueryResponse>
{ }