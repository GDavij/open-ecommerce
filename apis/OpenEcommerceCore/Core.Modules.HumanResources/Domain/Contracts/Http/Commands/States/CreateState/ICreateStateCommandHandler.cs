using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.States.CreateState;

internal interface ICreateStateCommandHandler
    : IRequestHandler<CreateStateCommand, CreateStateCommandResponse>
{ }