using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProductTag;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Queries.GetProductTag;

internal class GetProductTagQueryHandler : IGetProductTagQueryHandler
{
    private readonly IStockContext _dbContext;

    public GetProductTagQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetProductTagQueryResponse> Handle(GetProductTagQuery request, CancellationToken cancellationToken)
    {
        var productTag = await _dbContext.ProductTags
            .FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);

        if (productTag is null)
        {
            throw new InvalidProductTagException(request.Id);
        }

        return new GetProductTagQueryResponse
        {
            Id = productTag.Id,
            Name = productTag.Name
        };
    }
}