using Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaboratorSession;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.UserAccess.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CollaboratorsController : ControllerBase
{
   private readonly IMediator _mediator;

   public CollaboratorsController(IMediator mediator)
   {
      _mediator = mediator;
   }

   [HttpPost("Session")]
   public async Task<IActionResult> CreateCollaboratorSession([FromServices] AbstractValidator<CreateCollaboratorSessionCommand> validator, [FromBody] CreateCollaboratorSessionCommand command, CancellationToken cancellationToken)
   {
      var validationResult = validator.Validate(command);
      if (!validationResult.IsValid)
      {
         return BadRequest(validationResult.Errors);
      }

      return Ok(await _mediator.Send(command, cancellationToken));
   }
}