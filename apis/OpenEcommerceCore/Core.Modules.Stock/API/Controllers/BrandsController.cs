using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.Stock.API.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class BrandsController : ControllerBase 
{
   private readonly IMediator _mediator;
   
    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromServices] AbstractValidator<CreateBrandCommand> validator, [FromBody] CreateBrandCommand command, CancellationToken cancellationToken)
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
    public async Task<IActionResult> GetBrand([FromServices] AbstractValidator<GetBrandQuery> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBrandQuery
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
    public async Task<IActionResult> SearchBrand([FromServices] AbstractValidator<SearchBrandQuery> validator, [FromQuery] SearchBrandQuery query, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(query);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBrand([FromServices] AbstractValidator<UpdateBrandCommand> validator, [FromBody] UpdateBrandCommand command, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteBrand([FromServices] AbstractValidator<DeleteBrandCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBrandCommand
        {
            Id = id
        };

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command);

        return Ok();
    }
}