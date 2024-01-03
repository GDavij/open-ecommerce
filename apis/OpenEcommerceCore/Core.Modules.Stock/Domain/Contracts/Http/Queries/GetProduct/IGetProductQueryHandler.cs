using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;

public interface IGetProductQueryHandler
    : IRequestHandler<GetProductQuery, GetProductQueryResponse>
{ }