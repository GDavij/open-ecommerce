using Core.Modules.Shared.Domain.Models;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateCollaboratorForSector;

internal class AuthenticateCollaboratorForSectorCommandHandler : IAuthenticateCollaboratorForSectorCommandHandler
{
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public AuthenticateCollaboratorForSectorCommandHandler (ISecurityService securityService, IUserAccessContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _dbContext = dbContext;
        _securityService = securityService;
    }
    
    public async Task Consume(ConsumeContext<AuthenticateCollaboratorForSectorCommand> context)
    {
        Token token;
        bool isValidToken = _securityService.TryParseEncodedToken(context.Message.EncodedToken, out token);
       
        if (!isValidToken)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        if (token.Type != ETokenType.Collaborator)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        long tokenLastingTime = token.Exp - _dateTimeProvider.UtcNowOffset.ToUnixTimeSeconds();
        if (tokenLastingTime <= 0)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        var existentCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.Id == token.Id &&
                                      c.Deleted == false &&
                                      c.Sector == context.Message.Sector);

        if (existentCollaborator is null)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        byte[] password = Convert.FromBase64String(token.Password);

        bool isPasswordValid = existentCollaborator!.Password.SequenceEqual(password);
        if (!isPasswordValid)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        Identity identity = Identity.Create(existentCollaborator.CollaboratorModuleId);
        await context.RespondAsync(AuthenticationResult.IsAuthenticatedWithIdentity(identity));
    }
}