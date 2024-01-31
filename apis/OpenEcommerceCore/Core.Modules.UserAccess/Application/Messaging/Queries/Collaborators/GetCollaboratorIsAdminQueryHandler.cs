using Core.Modules.Shared.Domain.Helpers;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Queries.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Queries.Collaborators;

internal class GetCollaboratorIsAdminQueryHandler : IGetCollaboratorIsAdminQueryHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetCollaboratorIsAdminQueryHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task Consume(ConsumeContext<GetCollaboratorIsAdminCommand> context)
    {
        var collaboratorId = context.Message.Id;

        var existentCollaborator = await _dbContext.Collaborators
            .FirstAsync(c => c.CollaboratorModuleId == collaboratorId);

        var valueResult = new ValueTypeWrapper<bool>(existentCollaborator.IsAdmin);
        EvaluationResult<ValueTypeWrapper<bool>> result = new(valueResult);
        
        await context.RespondAsync(result);
    }
}