using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Core.Modules.HumanResources.API.Decorators.Authentication;

internal class IsAuthenticated : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var sp = httpContext.RequestServices;
        var requestAuthClient = sp.GetRequiredService<IRequestClient<AuthenticateCollaboratorForSectorCommand>>();
       
        bool hasToken = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues tokens);
        if (!hasToken)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string token = tokens.ToString();
        
        string[] parts = token.Split(' ');
        if (parts.Length != 2 || parts[0] != "Bearer")
        {
            context.Result = new UnauthorizedResult();
        }

        string auth = parts[1];
        var command = new AuthenticateCollaboratorForSectorCommand(auth, ECollaboratorSector.HumanResources);

        var cancellationToken = httpContext.RequestAborted;
        var authorizationResponse = await requestAuthClient.GetResponse<AuthenticationResult>(command, cancellationToken);
        if (!authorizationResponse.Message.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}