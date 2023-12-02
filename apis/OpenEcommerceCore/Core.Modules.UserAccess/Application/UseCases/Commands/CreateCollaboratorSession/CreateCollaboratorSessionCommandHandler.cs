using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Application.UseCases.Commands.CreateClientSession;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaboratorSession;

internal class CreateCollaboratorSessionCommandHandler : ICreateCollaboratorSessionCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateCollaboratorSessionCommandHandler(
        IUserAccessContext dbContext,
        ISecurityService securityService,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _securityService = securityService;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<ValidationResult<CreateCollaboratorSessionResponse>> Handle(CreateCollaboratorSessionCommand request, CancellationToken cancellationToken)
    {
        Collaborator? existentCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.Email == request.Email,cancellationToken);

        if (existentCollaborator is null)
        {
            return ValidationResult<CreateCollaboratorSessionResponse>.Error();
        }
        
        byte[] derivedPassword = await _securityService.DerivePassword(request.Password, existentCollaborator.SecurityKey, cancellationToken);
        bool isPasswordsEqual = derivedPassword.SequenceEqual(existentCollaborator.Password);
        if (isPasswordsEqual is false)
        {
            return ValidationResult<CreateCollaboratorSessionResponse>.Error();
        }
        
        Token token = Token.Create(
            existentCollaborator.Id,
            existentCollaborator.Password,
            ETokenType.Client,
            TokenExpiration.OneDayFromNow(_dateTimeProvider));

        string encodedToken = _securityService.EncodeToken(token);

        var response = new CreateCollaboratorSessionResponse(encodedToken);
        return ValidationResult<CreateCollaboratorSessionResponse>.Success(response);
    }
}