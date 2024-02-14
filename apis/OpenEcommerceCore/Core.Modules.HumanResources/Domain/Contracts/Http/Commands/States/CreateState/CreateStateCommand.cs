using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.States.CreateState;

public record CreateStateCommand(string Name, string ShortName) : IRequest<CreateStateCommandResponse>;