using System.Net;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Clients.CreateClientSession;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Commands.Clients.CreateClientSession;

internal class CreateClientSessionCommandHandler : ICreateClientSessionCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;
    
    public CreateClientSessionCommandHandler(IUserAccessContext dbContext, ISecurityService securityService, IUserAccessDateTimeProvider userAccessDateTimeProvider)
    {
        _dbContext = dbContext;
        _securityService = securityService;
        _userAccessDateTimeProvider = userAccessDateTimeProvider;
    }
    
    public async Task<ValidationResult<CreateClientSessionResponse>> Handle(CreateClientSessionCommand request, CancellationToken cancellationToken)
    {
        Client? existentClient = await _dbContext.Clients
            .FirstOrDefaultAsync(c => c.Email == request.Email && c.Deleted == false, cancellationToken);

        if (existentClient == null)
        {
            return ValidationResult<CreateClientSessionResponse>.Error(HttpStatusCode.NotFound);
        }

        byte[] derivedPassword = await _securityService.DerivePassword(request.Password, existentClient.SecurityKey, cancellationToken);
        bool isPasswordsEqual = derivedPassword.SequenceEqual(existentClient.Password);
        if (isPasswordsEqual is false)
        {
            return ValidationResult<CreateClientSessionResponse>.Error(HttpStatusCode.BadRequest);
        }
        
        Token token = Token.Create(
            existentClient.Id,
            existentClient.Password,
            ETokenType.Client,
            TokenExpirationTimeScope.OneDayFromNow(_userAccessDateTimeProvider));

        string encodedToken = _securityService.EncodeToken(token);

        var response = new CreateClientSessionResponse(encodedToken);
        return ValidationResult<CreateClientSessionResponse>.Success(response);
    }
}