using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;
using Core.Modules.UserAccess.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateClient;

internal class CreateClientCommandHandler : ICreateClientCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    
    public async Task Consume(ConsumeContext<CreateClientCommand> context)
    {
        // Implement Retry and verification for databaseConnection Health from start
        
        var existingClient = await _dbContext.Clients
            .FirstOrDefaultAsync(c => 
                c.Email == context.Message.Email &&
                c.Deleted == false);

        if (existingClient != null)
        {
            // Add More Error Safe Error handling
            return;
        }

        byte[] securityKey = _securityService.GenerateSecurityKey();
        byte[] derivedPassword = await _securityService.DerivePassword(
            context.Message.Password,
            securityKey,
            default);

        var client = Client.Create(
            Guid.NewGuid(),
            context.Message.ClientModuleId,
            context.Message.Email,
            derivedPassword,
            securityKey);

        _dbContext.Clients.Add(client);

        await _dbContext.SaveChangesAsync();
    }
}