using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.Models;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess.Auth;
using Core.Modules.UserAccess.Domain.Constants;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace Core.Modules.UserAccess.API.Decorators.Authentication;

internal class IsAuthenticated : Attribute, IAsyncAuthorizationFilter
{
    private IUserAccessContext _dbContext;
    private ISecurityService _securityService;
    private IUserAccessDateTimeProvider _dateTimeProvider;
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var sp = httpContext.RequestServices;
        
        _dbContext = sp.GetRequiredService<IUserAccessContext>();
        _securityService = sp.GetRequiredService<ISecurityService>();
        _dateTimeProvider = sp.GetRequiredService<IUserAccessDateTimeProvider>();
        
        bool hasToken = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues tokens);
        if (!hasToken)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string token = tokens.ToString();
        string auth = BearerToken.ParseAndGetEncodedToken(token);

        var cancellationToken = httpContext.RequestAborted;

        var authorizationResponse = await HandleAuthenticationForUserAccess(auth); 
        if (!authorizationResponse.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
        }
        
        context.HttpContext.Items
            .Add(HttpContextItems.Identity, authorizationResponse.Identity);
    }


    private async Task<AuthenticationResult> HandleAuthenticationForUserAccess(string encodedToken)
    {
                Token token;
                bool isValidToken = _securityService.TryParseEncodedToken(encodedToken, out token);
               
                if (!isValidToken)
                {
                    return AuthenticationResult.NotAuthenticated();
                }
                
                if (token.Type != ETokenType.Collaborator)
                {
                    return AuthenticationResult.NotAuthenticated();
                }
                
                long tokenLastingTime = token.Exp - _dateTimeProvider.UtcNowOffset.ToUnixTimeSeconds();
                if (tokenLastingTime <= 0)
                {
                    return AuthenticationResult.NotAuthenticated();
                }
        
                var existentAdministrator = await _dbContext.Collaborators
                    .Where(c => c.Id == token.Id && c.Deleted == false)
                    .FirstOrDefaultAsync(c => c.IsAdmin);
        
                if (existentAdministrator is null)
                {
                    return AuthenticationResult.NotAuthenticated();
                }
                
                byte[] password = Convert.FromBase64String(token.Password);
        
                bool isPasswordValid = existentAdministrator.Password.SequenceEqual(password);
                if (!isPasswordValid)
                {
                    return AuthenticationResult.NotAuthenticated();
                }
                
                Identity identity = Identity.Create(existentAdministrator.CollaboratorModuleId!.Value);
                return AuthenticationResult.IsAuthenticatedWithIdentity(identity);
    }
}