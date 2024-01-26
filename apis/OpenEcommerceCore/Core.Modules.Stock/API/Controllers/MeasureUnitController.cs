using Core.Modules.Shared.Domain.Constants;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.ListMeasureUnits;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.Stock.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("/api/stock/v1/[controller]")]
public class MeasureUnitController : ControllerBase
{
    private readonly IMediator _mediator;
       
    public MeasureUnitController(IMediator mediator)
    {
        _mediator = mediator; 
    }
        
    [HttpPost]
    public async Task<IActionResult> CreateMeasureUnit([FromServices] AbstractValidator<CreateMeasureUnitCommand> validator, [FromBody] CreateMeasureUnitCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var resource = await _mediator.Send(command, cancellationToken);
        return Ok(resource);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetMeasureUnit([FromServices] AbstractValidator<GetMeasureUnitQuery> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetMeasureUnitQuery
        {
            Id = id
        };

        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> ListMeasureUnits([FromServices] AbstractValidator<ListMeasureUnitsQuery> validator, [FromQuery] ListMeasureUnitsQuery query, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMeasureUnit([FromServices] AbstractValidator<UpdateMeasureUnitCommand> validator, [FromBody] UpdateMeasureUnitCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var resource = await _mediator.Send(command, cancellationToken);
        return Ok(resource);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteMeasureUnit([FromServices] AbstractValidator<DeleteMeasureUnitCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMeasureUnitCommand
        {
            Id = id
        };

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

}