using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Brand;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteBrand;

internal class UpdateBrandCommandHandler : IUpdateBrandCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;

    public UpdateBrandCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }

    public async Task<UpdateBrandCommandResponse> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var existentBrand = await _dbContext.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (existentBrand is null)
        {
            throw new InvalidBrandException(request.Id);
        }

        existentBrand.Name = request.Name;
        existentBrand.Description = request.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: Add Retry with Polly
        await _publishEndpoint.Publish(BrandUpdatedIntegrationEvent.CreateEvent(existentBrand.MapToBrandDto()));

        return UpdateBrandCommandResponse.Respond($"brands/{existentBrand.Id}", _configService);
    }
}