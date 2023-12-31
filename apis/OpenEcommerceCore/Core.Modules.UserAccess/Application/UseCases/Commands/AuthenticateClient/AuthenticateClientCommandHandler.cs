using Core.Modules.Shared.Domain.Models;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateClient;

internal class AuthenticateClientCommandHandler : IAuthenticateClientCommandHandler
{
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;

    public AuthenticateClientCommandHandler(ISecurityService securityService, IUserAccessContext dbContext, IUserAccessDateTimeProvider userAccessDateTimeProvider)
    {
        _securityService = securityService;
        _dbContext = dbContext;
        _userAccessDateTimeProvider = userAccessDateTimeProvider;
    }

    public async Task Consume(ConsumeContext<AuthenticateClientCommand> context)
    {
        Token token;
        bool isValidToken = _securityService.TryParseEncodedToken(context.Message.EncodedToken, out token);

        if (!isValidToken)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        if (token.Type != ETokenType.Client)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        long tokenLastingTime = token.Exp - _userAccessDateTimeProvider.UtcNowOffset.ToUnixTimeSeconds();
        if (tokenLastingTime <= 0)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        var existentClient = await _dbContext.Clients
            .FirstOrDefaultAsync(c => c.Id == token.Id && c.Deleted == false);

        if (existentClient is null)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        byte[] password = Convert.FromBase64String(token.Password);

        bool isPasswordValid = existentClient.Password.SequenceEqual(password);
        if (!isPasswordValid)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        Identity identity = Identity.Create(existentClient.ClientModuleId);
        await context.RespondAsync(AuthenticationResult.IsAuthenticatedWithIdentity(identity));
    }
}