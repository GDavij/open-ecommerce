using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;

public record SearchBrandQuery : IRequest<SearchBrandQueryResponse>
{
    public int Page { get; init; }
    public string? SearchTerm { get; init; }
}