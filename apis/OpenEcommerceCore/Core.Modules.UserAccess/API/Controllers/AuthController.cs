using Core.Modules.Shared.Domain.Constants;
using Core.Modules.UserAccess.API.Decorators.Authentication;
using Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Auth.GetPermissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Core.Modules.UserAccess.API.Controllers;

[ApiExplorerSettings(GroupName = SwaggerApiGroups.V1)]
[ApiController]
[Route("api/user-access/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [IsAllowed]
    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
    {
        var permissions = await _mediator.Send(new GetPermissionsQuery(), cancellationToken);
        return Ok(permissions);
    }
}