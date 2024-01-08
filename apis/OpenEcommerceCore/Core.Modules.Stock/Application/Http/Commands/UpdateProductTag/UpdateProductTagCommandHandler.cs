using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProductTag;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.Exceptions.ProductTag;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using Core.Modules.Stock.Domain.IntegrationEvents.Tags;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.UpdateProductTag;

internal class UpdateProductTagCommandHandler : IUpdateProductTagCommandHandler 
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;

    public UpdateProductTagCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }

    public async Task<UpdateProductTagCommandResponse> Handle(UpdateProductTagCommand request, CancellationToken cancellationToken)
    {
        var existentTag = await _dbContext.ProductTags
            .FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);

        if (existentTag is null)
        {
            throw new InvalidProductTagException(request.Id);
        }

        var tagWithSameNameToUpdate = await _dbContext.ProductTags
            .FirstOrDefaultAsync(pt => pt.Name == request.Name, cancellationToken);

        if (tagWithSameNameToUpdate is not null)
        {
            throw new AlreadyExistentProductTagException(tagWithSameNameToUpdate.Name);
        }

        existentTag.Name = request.Name;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //TODO: Add Retry with Polly
        _publishEndpoint.Publish(ProductTagUpdatedIntegrationEvent.CreateEvent(existentTag.MapToProductTagDto()));
        
        return UpdateProductTagCommandResponse.Respond($"tags/{existentTag.Id}", _configService);
    }
}