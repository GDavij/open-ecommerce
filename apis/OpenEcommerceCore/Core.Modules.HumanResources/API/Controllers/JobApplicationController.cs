using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SendJobApplication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchJobApplication;
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

    [HttpGet]
    [IsAuthenticated]
    public async Task<IActionResult> SearchJobApplications([FromServices] AbstractValidator<SearchJobApplicationsQuery> validator, [FromQuery] SearchJobApplicationsQuery query, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(query, cancellationToken));
    }
}