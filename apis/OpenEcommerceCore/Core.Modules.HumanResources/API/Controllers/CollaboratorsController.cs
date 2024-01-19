using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Application.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.HumanResources.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollaboratorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [HttpPost]
    [IsAuthenticated]
    public async Task<IActionResult> CreateCollaborator([FromServices] AbstractValidator<CreateCollaboratorCommand> validator, [FromBody] CreateCollaboratorCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }
}