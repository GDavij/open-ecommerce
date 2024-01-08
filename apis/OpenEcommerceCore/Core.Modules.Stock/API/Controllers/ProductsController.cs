using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.Stock.API.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ProductsController : ControllerBase
{ 
   private readonly IMediator _mediator;
   
    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromServices] AbstractValidator<CreateProductCommand> validator, [FromBody] CreateProductCommand command, CancellationToken cancellationToken)
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
    public async Task<IActionResult> GetProduct([FromServices] AbstractValidator<GetProductQuery> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new GetProductQuery
        {
            Id = id
        };
   
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
   
        var product = await _mediator.Send(command, cancellationToken);
        return Ok(product);
    }
   
    [HttpGet]
    public async Task<IActionResult> SearchProducts([FromServices] AbstractValidator<SearchProductQuery> validator, [FromQuery] SearchProductQuery query, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
   
        return Ok(await _mediator.Send(query));
    }
   
    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromServices] AbstractValidator<UpdateProductCommand> validator, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
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
    public async Task<IActionResult> DeleteProduct([FromServices] AbstractValidator<DeleteProductCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand
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
    
    [HttpPost]
    [Route("images")]
    public async Task<IActionResult> AddImageToProduct([FromServices] AbstractValidator<AddImageToProductCommand> validator, [FromForm] AddImageToProductCommand command, CancellationToken cancellationToken)
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
    [Route("images/{id}")]
    public async Task<IActionResult> RemoveImageFromProduct([FromServices] AbstractValidator<RemoveImageFromProductCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveImageFromProductCommand
        {
            Id = id
        };

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}