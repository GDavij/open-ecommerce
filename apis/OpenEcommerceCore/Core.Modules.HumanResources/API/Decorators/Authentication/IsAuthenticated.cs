using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.Modules.HumanResources.API.Decorators.Authentication;

internal class IsAuthenticated : IAuthorizationRequirement
{ }

internal class IsAuthenticatedHandler : AuthorizationHandler<IsAuthenticated>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestClient<AuthenticateCollaboratorForSectorCommand> _requestAuthClient;

    public IsAuthenticatedHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthenticated requirement)
    {
        bool hasToken = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("token", out StringValues token);
        if (!hasToken)
        {
            context.Fail();
            return;
        }

        var command = new AuthenticateCollaboratorForSectorCommand(token, ECollaboratorSector.HumanResources);

        var cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;
        var authorizationResponse = await _requestAuthClient.GetResponse<AuthenticationResult>(command, cancellationToken);
        if (!authorizationResponse.Message.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}

