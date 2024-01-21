using Core.Modules.Shared.Domain.Helpers;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.GetCollaboratorIsDeleted;

internal class GetCollaboratorIsDeletedCommandHandler : IGetCollaboratorIsDeletedCommandHandler
{
    private readonly IUserAccessContext _dbContext;

    public GetCollaboratorIsDeletedCommandHandler(IUserAccessContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<GetCollaboratorIsDeletedCommand> context)
    {
        var collaboratorId = context.Message.Id;
        
        var existentCollaborator = await _dbContext.Collaborators
            .FirstAsync(c => c.CollaboratorModuleId == collaboratorId);
        
        var valueResult = new ValueTypeWrapper<bool>(existentCollaborator.Deleted);
        EvaluationResult<ValueTypeWrapper<bool>> result = new(valueResult);

        await context.RespondAsync(result);
    }
}