using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProductTag;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProductTag;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProductTag;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProductTag;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.ListProductTags;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.Stock.API.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class TagsController : ControllerBase
{

    
    private readonly IMediator _mediator;
    
    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromServices] AbstractValidator<CreateProductTagCommand> validator, [FromBody] CreateProductTagCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
    
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
    
        return Ok(await _mediator.Send(command, cancellationToken));
    }
    
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTag([FromServices] AbstractValidator<GetProductTagQuery> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductTagQuery 
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
    public async Task<IActionResult> ListTags([FromServices] AbstractValidator<ListProductTagsQuery> validator, [FromQuery] ListProductTagsQuery query, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
    
        return Ok(await _mediator.Send(query, cancellationToken));
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateTag([FromServices] AbstractValidator<UpdateProductTagCommand> validator, [FromBody] UpdateProductTagCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
    
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
    
        return Ok(await _mediator.Send(command, cancellationToken));
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteTag([FromServices] AbstractValidator<DeleteProductTagCommand> validator,
        [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductTagCommand
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