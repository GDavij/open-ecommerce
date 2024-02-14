using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.States.CreateState;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Exceptions.State;
using Core.Modules.Shared.Domain.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.States.CreateState;

internal class CreateStateCommandHandler : ICreateStateCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IAppConfigService _configService;

    public CreateStateCommandHandler(IHumanResourcesContext dbContext, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _configService = configService;
    }
    
    public async Task<CreateStateCommandResponse> Handle(CreateStateCommand request, CancellationToken cancellationToken)
    {
        var existentState = await _dbContext.States
            .FirstOrDefaultAsync(x => 
                EF.Functions.ILike(x.Name, request.Name) || EF.Functions.ILike(x.ShortName, request.ShortName), cancellationToken);

        if (existentState is not null)
        {
            throw new AlreadyExistentStateException(existentState.Name, existentState.ShortName);
        }

        var state = new State
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            ShortName = request.ShortName,
            Addresses = new List<Address>()
        };

        _dbContext.States.Add(state);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreateStateCommandResponse.Respond(state.Id, _configService);
    }
}