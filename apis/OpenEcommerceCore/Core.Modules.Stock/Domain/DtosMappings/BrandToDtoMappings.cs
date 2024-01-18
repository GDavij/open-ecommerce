using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;
using Core.Modules.Stock.Domain.Entities;

namespace Core.Modules.Stock.Domain.DtosMappings;

internal static class BrandToDtoMappings
{
    public static BrandDto MapToBrandDto(this Brand brand)
    {
        return BrandDto.Create(
            brand.Id,
            brand.Name,
            brand.Description,
            brand.Products.Select(p => p.Id).ToList());
    }
}