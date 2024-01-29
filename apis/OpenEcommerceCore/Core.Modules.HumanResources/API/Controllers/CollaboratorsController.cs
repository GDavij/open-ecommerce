using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.UpdateCollaborator;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries;
using Core.Modules.Shared.Domain.Constants;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.HumanResources.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("api/human-resources/v1/[controller]")]
public class CollaboratorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollaboratorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> GetCollaboratorById([FromServices] AbstractValidator<GetCollaboratorQuery> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new GetCollaboratorQuery(id);
        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }
        
        return Ok(await _mediator.Send(command, cancellationToken));
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
    
    [HttpPut]
    [IsAuthenticated]
    public async Task<IActionResult> UpdateCollaborator([FromServices] AbstractValidator<UpdateCollaboratorCommand> validator, [FromBody] UpdateCollaboratorCommand command, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> DeleteCollaborator([FromServices] AbstractValidator<DeleteCollaboratorCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCollaboratorCommand(id);

        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}