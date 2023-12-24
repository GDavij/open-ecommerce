using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Stock.Application.IntegrationEvents.Models.Mappings;

internal static class BrandToDtoMappings
{
    public static BrandDto MapToBrandDto(this Domain.Entities.Brand brand)
    {
        return BrandDto.Create(
            brand.Id,
            brand.Name,
            brand.Description,
            brand.Products.Select(p => p.Id).ToList());
    }
}