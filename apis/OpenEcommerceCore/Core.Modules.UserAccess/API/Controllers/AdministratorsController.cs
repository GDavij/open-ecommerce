using Core.Modules.Shared.Domain.Constants;
using Core.Modules.UserAccess.API.Decorators.Authentication;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.CreateAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.DeleteAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.UpdateAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.Modules.UserAccess.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("api/user-access/v1/[controller]")]
public class AdministratorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdministratorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromServices] AbstractValidator<CreateAdministratorCommand> validator, [FromBody] PartialCreateAdministratorCommand requestSchema, [FromHeader] string? authorization, CancellationToken cancellationToken)
    {
        var command = new CreateAdministratorCommand
        {
            Email = requestSchema.Email,
            Password = requestSchema.Password,
            Authorization = authorization 
        };
        
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode((int)result.Code, result);
    }
    
    [HttpGet]
    [IsAuthenticated]
    public async Task<IActionResult> List([FromServices] AbstractValidator<ListAdministratorsQuery> validator, [FromQuery] ListAdministratorsQuery query, CancellationToken cancellationToken)
    {

        var validationResults = validator.Validate(query);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        var result = await _mediator.Send(query, cancellationToken);
        return StatusCode((int)result.Code, result);
    }

    [HttpPut("{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> Update([FromServices] AbstractValidator<UpdateAdministratorCommand> validator, [FromBody] PartialUpdateAdministratorCommand requestCommand, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new UpdateAdministratorCommand
        {
            Id = id,
            Email = requestCommand.Email,
            Password = requestCommand.Password
        };

        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode((int)result.Code, result);
    }

    [HttpDelete("{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> Delete([FromServices] AbstractValidator<DeleteAdministratorCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteAdministratorCommand(id);

        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode((int)result.Code, result);
    }
}