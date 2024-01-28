using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.GetDeletedCollaboratorsIds;

internal class GetDeletedCollaboratorsIdsCommandHandler : IGetDeletedCollaboratorsIdsCommandHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetDeletedCollaboratorsIdsCommandHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetDeletedCollaboratorsIdsCommand> context)
    {
        var deletedCollaborators = await _dbContext.Collaborators
            .Where(c => c.Deleted)
            .Select(c => c.CollaboratorModuleId)
            .ToListAsync();

        await context.RespondAsync(new EvaluationResult<List<Guid>>(deletedCollaborators));
    }
}