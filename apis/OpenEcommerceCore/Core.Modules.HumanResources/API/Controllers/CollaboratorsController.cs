using Core.Modules.HumanResources.Application.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.HumanResources.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollaboratorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollaborator([FromServices] CreateCollaboratorCommandValidator validator, [FromBody] CreateCollaboratorCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }
}