using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.AddContracts;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteContract;
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
    public async Task<IActionResult> BreakContract([FromServices] AbstractValidator<BreakContractCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new BreakContractCommand(id);
        
        var validationResult = validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    [IsAuthenticated]
    public async Task<IActionResult> AddContracts([FromServices] AbstractValidator<AddContractsCommand> validator, [FromBody] AddContractsCommand command, CancellationToken cancellationToken)
    {
        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> DeleteContract([FromServices] AbstractValidator<DeleteContractCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteContractCommand(id);
        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}