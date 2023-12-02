using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateClientSession;

internal class CreateClientSessionCommandHandler : ICreateClientSessionCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateClientSessionCommandHandler(IUserAccessContext dbContext, ISecurityService securityService, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _securityService = securityService;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<ValidationResult<CreateClientSessionResponse>> Handle(CreateClientSessionCommand request, CancellationToken cancellationToken)
    {
        Client? existentClient = await _dbContext.Clients
            .FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);

        if (existentClient == null)
        {
            return ValidationResult<CreateClientSessionResponse>.Error();
        }

        byte[] derivedPassword = await _securityService.DerivePassword(request.Password, existentClient.SecurityKey, cancellationToken);
        bool isPasswordsEqual = derivedPassword.SequenceEqual(existentClient.Password);
        if (isPasswordsEqual is false)
        {
            return ValidationResult<CreateClientSessionResponse>.Error();
        }
        
        Token token = Token.Create(
            existentClient.Id,
            existentClient.Password,
            ETokenType.Client,
            TokenExpiration.OneDayFromNow(_dateTimeProvider));

        string encodedToken = _securityService.EncodeToken(token);

        var response = new CreateClientSessionResponse(encodedToken);
        return ValidationResult<CreateClientSessionResponse>.Success(response);
    }
}