using Core.Modules.Stock.Application.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("products")]
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
    [Route("products/{id}")]
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
    [Route("products")]
    public async Task<IActionResult> SearchProducts([FromServices] AbstractValidator<SearchProductQuery> validator,
        [FromQuery] SearchProductQuery query)
    {
        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(query));
    }

    [HttpPut]
    [Route("products")]
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
    [Route("products/{id}")]
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
    [Route("brands")]
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
    [Route("brands/{id}")]
    public async Task<IActionResult> GetBrand([FromServices] AbstractValidator<GetBrandQuery> validator, [FromRoute] Guid id)
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

        return Ok(await _mediator.Send(query));
    }

    [HttpGet]
    [Route("brands")]
    public async Task<IActionResult> SearchBrand([FromServices] AbstractValidator<SearchBrandQuery> validator, [FromQuery] SearchBrandQuery query)
    {
        var validationResult = validator.Validate(query);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(query));
    }

    [HttpPut]
    [Route("brands")]
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
    [Route("brands/{id}")]
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

    [HttpPost]
    [Route("products/images")]
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
    [Route("products/images/{id}")]
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

    [HttpPost]
    [Route("measure-units")]
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

    [HttpPut]
    [Route("measure-units")]
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
    [Route("measure-units/{id}")]
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
