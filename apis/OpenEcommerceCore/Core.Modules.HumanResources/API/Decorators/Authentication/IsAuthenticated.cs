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

        context.Succeed(requirement);

    }
}

