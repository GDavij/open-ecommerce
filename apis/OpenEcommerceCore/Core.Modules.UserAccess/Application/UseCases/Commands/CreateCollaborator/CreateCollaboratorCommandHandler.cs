using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaborator;

internal class CreateCollaboratorCommandHandler : ICreateCollaboratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;

    public CreateCollaboratorCommandHandler(IUserAccessContext dbContext, ISecurityService securityService)
    {
        _dbContext = dbContext;
        _securityService = securityService;
    }

    public async Task Consume(ConsumeContext<CreateCollaboratorCommand> context)
    {
        // Implement Retry and verification for databaseConnection Health from start
        
        var existentCollaborator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c =>
                c.Email == context.Message.Email &&
                c.Deleted == false);

        if (existentCollaborator != null)
        {
            // Add More Error Safe Error handling
            return;
        }

        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            context.Message.Password,
            securityKey,
            default);

        var collaborator = Collaborator.Create(
            Guid.NewGuid(),
            context.Message.CollaboratorModuleId,
            context.Message.Email,
            derivedPassword,
            securityKey,
            context.Message.CollaboratorSector);

        _dbContext.Collaborators.Add(collaborator);

        await _dbContext.SaveChangesAsync();
    }
}