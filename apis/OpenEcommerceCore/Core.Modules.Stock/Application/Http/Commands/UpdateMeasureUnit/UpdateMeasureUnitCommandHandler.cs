using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.MeasureUnit;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.UpdateMeasureUnit;

internal class UpdateMeasureUnitCommandHandler : IUpdateMeasureUnitCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;

    public UpdateMeasureUnitCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }

    public async Task<UpdateMeasureUnitCommandResponse> Handle(UpdateMeasureUnitCommand request, CancellationToken cancellationToken)
    {
        var existentMeasureUnit = await _dbContext.MeasureUnits
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (existentMeasureUnit is null)
        {
            throw new InvalidMeasureUnitException(request.Id);
        }

        existentMeasureUnit.Name = request.Name;
        existentMeasureUnit.ShortName = request.ShortName;
        existentMeasureUnit.Symbol = request.Symbol;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //TODO: Add Retry with Polly
        await _publishEndpoint.Publish(MeasureUnitUpdatedIntegrationEvent.CreateEvent(existentMeasureUnit.MapToMeasureUnitDto()));
        
        return UpdateMeasureUnitCommandResponse.Respond($"measureUnits/{existentMeasureUnit.Id}", _configService);
    }
}