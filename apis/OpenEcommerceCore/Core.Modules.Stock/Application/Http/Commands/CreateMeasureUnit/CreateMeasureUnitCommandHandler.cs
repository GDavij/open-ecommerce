using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Exceptions.MeasureUnit;
using Core.Modules.Stock.Domain.IntegrationEvents.MeasureUnit;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.CreateMeasureUnit;

internal class CreateMeasureUnitCommandHandler : ICreateMeasureUnitCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;

    public CreateMeasureUnitCommandHandler(
        IStockContext dbContext,
        IPublishEndpoint publishEndpoint,
        IAppConfigService configService)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }
    
    public async Task<CreateMeasureUnitCommandResponse> Handle(CreateMeasureUnitCommand request, CancellationToken cancellationToken)
    {
        var existentMeasureUnit = await _dbContext.MeasureUnits
            .FirstOrDefaultAsync(m => 
                m.Name == request.Name ||
                m.ShortName == request.ShortName,
                cancellationToken);

        if (existentMeasureUnit is not null)
        {
            throw new AlreadyExistentMeasureUnitException(existentMeasureUnit.Name, existentMeasureUnit.ShortName);
        }

        MeasureUnit measureUnit = MeasureUnit.Create(request.Name, request.ShortName, request.Symbol);
        _dbContext.MeasureUnits.Add(measureUnit);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //TODO: Implement Retry With Polly
        await _publishEndpoint.Publish(MeasureUnitCreatedIntegrationEvent.CreateEvent(measureUnit.MapToMeasureUnitDto()));

        return CreateMeasureUnitCommandResponse.Respond($"measureUnits/{measureUnit.Id}", _configService);
    }
}