using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Collaborators;
using MassTransit;

namespace Core.Modules.UserAccess.Application.Messaging.Queries.Collaborators;

internal class GetCollaboratorsIdsFromSectorQueryHandler : IGetCollaboratorsIdsFromSectorQueryHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetCollaboratorsIdsFromSectorQueryHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetCollaboratorsIdsFromSectorQuery> context)
    {
        var collaborators = _dbContext.Collaborators
            .Where(c => c.Sectors.Contains(context.Message.Sector))
            .Select(c => c.CollaboratorModuleId!.Value)
            .ToHashSet();

        var result = new EvaluationResult<HashSet<Guid>>(collaborators);
        await context.RespondAsync(result);
    }
}