using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProductTag;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.Exceptions.ProductTag;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using Core.Modules.Stock.Domain.IntegrationEvents.Tags;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.CreateProductTag;

internal class CreateProductTagCommandHandler : ICreateProductTagCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;

    public CreateProductTagCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }

    public async Task<CreateProductTagCommandResponse> Handle(CreateProductTagCommand request, CancellationToken cancellationToken)
    {
        var existentProductTag = await _dbContext.ProductTags
            .FirstOrDefaultAsync(pt => pt.Name == request.Name, cancellationToken);

        if (existentProductTag is not null)
        {
            throw new AlreadyExistentProductTagException(request.Name);
        }
        var productTag = ProductTag.Create(request.Name);

        _dbContext.ProductTags.Add(productTag);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: Add Retry with Polly
        await _publishEndpoint.Publish<ProductTagCreatedIntegrationEvent>(ProductTagCreatedIntegrationEvent.CreateEvent(productTag.MapToProductTagDto()));
        
        return CreateProductTagCommandResponse.Respond($"tags/{productTag.Id}", _configService);
    }
}