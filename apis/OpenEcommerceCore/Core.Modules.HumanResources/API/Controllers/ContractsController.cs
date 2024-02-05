using Core.Modules.HumanResources.API.Decorators.Authentication;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddWorkHourToContributionYear;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.BreakContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.DeleteContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.GetContract;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.SearchContracts;
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

    [HttpGet]
    [IsAuthenticated]
    public async Task<IActionResult> SearchContracts([FromServices] AbstractValidator<SearchContractsQuery> validator, [FromQuery] SearchContractsQuery query, CancellationToken cancellationToken)
    {
        var validationResults = validator.Validate(query);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> GetContract([FromServices] AbstractValidator<GetContractQuery> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetContractQuery(id);
        var validationResults = validator.Validate(query);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        return Ok(await _mediator.Send(query, cancellationToken));
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
    
    [HttpPost("{id}/work-hours")]
    [IsAuthenticated]
    public async Task<IActionResult> AddWorkHourToContributionYear([FromServices] AbstractValidator<AddWorkHourToContributionYearCommand> validator, [FromRoute] Guid id, [FromBody] AddWorkHourToContributionYearCommand.WorkHourRequestSchema workHour, CancellationToken cancellationToken)
    {
        var command = new AddWorkHourToContributionYearCommand
        {
            ContractId = id,
            WorkHour = workHour
        };
        
        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("work-hours/{id}")]
    [IsAuthenticated]
    public async Task<IActionResult> RemoveWorkHourFromContributionYear([FromServices] AbstractValidator<RemoveWorkHourFromContributionYearCommand> validator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveWorkHourFromContributionYearCommand(id);
        var validationResults = validator.Validate(command);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
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