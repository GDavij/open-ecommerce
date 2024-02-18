using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Administrators;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Administrators;
using MassTransit;

namespace Core.Modules.UserAccess.Application.Messaging.Queries.Administrators;

internal class GetAdministratorsIdsQueryHandler : IGetAdministratorsIdsQueryHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetAdministratorsIdsQueryHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetAdministratorsIdsQuery> context)
    {
        var administrators = _dbContext.Collaborators
            .Where(c => !c.Deleted && c.IsAdmin)
            .Select(c => c.Id)
            .ToHashSet();

        await context.RespondAsync(new EvaluationResult<HashSet<Guid>>(administrators));
    }
}