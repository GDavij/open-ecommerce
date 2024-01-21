using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.UpdateCollaborator;

internal interface IUpdateCollaboratorCommandHandler
    : IRequestHandler<UpdateCollaboratorCommand, UpdateCollaboratorCommandResponse>
{ }