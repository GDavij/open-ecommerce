using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Exceptions.Brand;
using Core.Modules.Stock.Domain.IntegrationEvents.Brand;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.CreateBrand;

internal class CreateBrandCommandHandler : ICreateBrandCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;

    public CreateBrandCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }

    public async Task<CreateBrandCommandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var existentBrand = await _dbContext.Brands
            .FirstOrDefaultAsync(b => b.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

        if (existentBrand is not null)
        {
            throw new AlreadyExistentBrandException(request.Name);
        }

        var brand = Brand.Create(request.Name, request.Description);

        _dbContext.Brands.Add(brand);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //TODO: Must add Retry with Polly
        await _publishEndpoint.Publish(BrandCreatedIntegrationEvent.CreateEvent(brand.MapToBrandDto()));
        
        return CreateBrandCommandResponse.Respond($"brands/{brand.Id}", _configService);
    }
}