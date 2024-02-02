using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SendJobApplication;
using Core.Modules.Shared.Domain.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.HumanResources.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("api/human-resources/v1/[controller]")]
public class JobApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SendJobApplication([FromServices] AbstractValidator<SendJobApplicationCommand> validator, [FromForm] SendJobApplicationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}