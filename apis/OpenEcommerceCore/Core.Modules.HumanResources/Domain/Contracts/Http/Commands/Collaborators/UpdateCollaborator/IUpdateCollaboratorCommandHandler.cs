using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.UpdateCollaborator;

internal interface IUpdateCollaboratorCommandHandler
    : IRequestHandler<UpdateCollaboratorCommand, UpdateCollaboratorCommandResponse>
{ }