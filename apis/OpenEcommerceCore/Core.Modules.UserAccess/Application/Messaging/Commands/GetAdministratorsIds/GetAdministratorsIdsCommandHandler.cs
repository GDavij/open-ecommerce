using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.GetAdministratorsIds;

internal class GetAdministratorsIdsCommandHandler : IGetAdministratorsIdsCommandHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetAdministratorsIdsCommandHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetAdministratorsIdsCommand> context)
    {
        var administrators = await _dbContext.Collaborators
            .Where(c => !c.Deleted && c.IsAdmin)
            .Select(c => c.CollaboratorModuleId)
            .ToListAsync();

        await context.RespondAsync(new EvaluationResult<List<Guid>>(administrators));
    }
}