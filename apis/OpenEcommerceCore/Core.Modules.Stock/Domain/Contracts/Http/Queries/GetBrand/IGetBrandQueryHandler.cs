using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;

public interface IGetBrandQueryHandler
    : IRequestHandler<GetBrandQuery, GetBrandQueryResponse>
{ }