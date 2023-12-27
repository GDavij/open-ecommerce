using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<UpdateProductCommandResponse>
{
    public Guid ProductId { get; init; }
    public Guid BrandId { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public string? Sku { get; init; }
    public string Ean { get; init; }
    public string? Upc { get; init; }
    public decimal Price { get; init; }
    public int StockUnitCount { get; init; }
    public List<Guid> TagsIds { get; init; }
    public List<ProductDetailUpdateRequestPayload> Measurements { get; init; }
    public List<ProductDetailUpdateRequestPayload> TechnicalDetails { get; init; }
    public List<ProductDetailUpdateRequestPayload> OtherDetails { get; init; }
}

public record ProductDetailUpdateRequestPayload
{ 
    public int ShowOrder { get; init; } 
    public string Name { get; init; } 
    public string Value { get; init; } 
    public Guid? MeasureUnitId { get; init; } 
}
