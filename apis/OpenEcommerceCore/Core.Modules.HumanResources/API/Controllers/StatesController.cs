using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.States.CreateState;
using Core.Modules.Shared.Domain.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.HumanResources.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("api/human-resources/v1/[controller]")]
public class StatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [IsAuthenticated]
    public async Task<IActionResult> CreateState([FromServices] AbstractValidator<CreateStateCommand> validator, [FromBody] CreateStateCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }
}