using Core.Modules.Stock.Application.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    [Route("products/create")]
    public async Task<IActionResult> CreateProduct([FromServices] AbstractValidator<CreateProductCommand> validator, [FromBody] CreateProductCommand command)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command);
        return Ok();
    }
}