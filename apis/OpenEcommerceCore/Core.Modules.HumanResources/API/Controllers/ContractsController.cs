using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;
using Core.Modules.Shared.Domain.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.HumanResources.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("api/human-resources/v1/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("break/{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> BreakContract([FromServices] AbstractValidator<BreakContractCommand> validator, [FromRoute] Guid id)
    {
        var command = new BreakContractCommand(id);
        
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command);
        return Ok();
    }
}