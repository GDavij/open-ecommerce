using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Collaborators;
using MassTransit;

namespace Core.Modules.UserAccess.Application.Messaging.Queries.Collaborators;

internal class GetDeletedCollaboratorsIdsQueryHandler : IGetDeletedCollaboratorsIdsQueryHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetDeletedCollaboratorsIdsQueryHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetDeletedCollaboratorsIdsCommand> context)
    {
        var deletedCollaborators = _dbContext.Collaborators
            .Where(c => c.Deleted)
            .Select(c => c.CollaboratorModuleId)
            .ToHashSet();

        await context.RespondAsync(new EvaluationResult<HashSet<Guid>>(deletedCollaborators));
    }
}