using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    [HttpPost]
    [Route("products/create")]
    public async Task<IActionResult> CreateProduct([FromServices] ICreateProductCommandHandler commandHandler, [FromBody] CreateProductCommand command)
    {
        await commandHandler.Handle(command, default);
        return Ok();
    }
}