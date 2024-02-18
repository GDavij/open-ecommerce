using System.Net;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Collaborators.CreateCollaboratorSession;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Helpers;
using Core.Modules.UserAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Commands.Collaborators.CreateCollaboratorSession;

internal class CreateCollaboratorSessionCommandHandler : ICreateCollaboratorSessionCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;

    public CreateCollaboratorSessionCommandHandler(
        IUserAccessContext dbContext,
        ISecurityService securityService,
        IUserAccessDateTimeProvider userAccessDateTimeProvider)
    {
        _dbContext = dbContext;
        _securityService = securityService;
        _userAccessDateTimeProvider = userAccessDateTimeProvider;
    }
    
    public async Task<ValidationResult<CreateCollaboratorSessionResponse>> Handle(CreateCollaboratorSessionCommand request, CancellationToken cancellationToken)
    {
        Collaborator? existentCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.Email == request.Email && c.Deleted == false, cancellationToken);

        if (existentCollaborator is null)
        {
            return ValidationResult<CreateCollaboratorSessionResponse>.Error(HttpStatusCode.NotFound);
        }
        
        byte[] derivedPassword = await _securityService.DerivePassword(request.Password, existentCollaborator.SecurityKey, cancellationToken);
        bool isPasswordsEqual = derivedPassword.SequenceEqual(existentCollaborator.Password);
        if (isPasswordsEqual is false)
        {
            return ValidationResult<CreateCollaboratorSessionResponse>.Error(HttpStatusCode.BadRequest);
        }
        
        existentCollaborator.LastLogin = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        Token token = Token.Create(
            existentCollaborator.Id,
            existentCollaborator.Password,
            ETokenType.Collaborator,
            TokenExpiration.OneDayFromNow(_userAccessDateTimeProvider));
        
        string encodedToken = _securityService.EncodeToken(token);

        var response = new CreateCollaboratorSessionResponse(encodedToken);
        return ValidationResult<CreateCollaboratorSessionResponse>.Success(response);
    }
}