using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using MassTransit;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.GetCollaboratorsIdsFromSector;

internal class GetCollaboratorsIdsFromSectorCommandHandler : IGetCollaboratorsIdsFromSectorCommandHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetCollaboratorsIdsFromSectorCommandHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<Shared.Messaging.Commands.UserAccess.GetCollaboratorsIdsFromSector> context)
    {
        var collaborators = _dbContext.Collaborators
            .Where(c => c.Sectors.Contains(context.Message.Sector))
            .Select(c => c.CollaboratorModuleId)
            .ToHashSet();

        var result = new EvaluationResult<HashSet<Guid>>(collaborators);
        await context.RespondAsync(result);
    }
}