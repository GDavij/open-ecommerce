using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
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

    public async Task Consume(ConsumeContext<GetDeletedCollaboratorsIdsCommandQuery> context)
    {
        var deletedCollaborators = _dbContext.Collaborators
            .Where(c => c.Deleted && c.CollaboratorModuleId != null)
            .Select(c => c.CollaboratorModuleId!.Value)
            .ToHashSet();

        await context.RespondAsync(new EvaluationResult<HashSet<Guid>>(deletedCollaborators));
    }
}