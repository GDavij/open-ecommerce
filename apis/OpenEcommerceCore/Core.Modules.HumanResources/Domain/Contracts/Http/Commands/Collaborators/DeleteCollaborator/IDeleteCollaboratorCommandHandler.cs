using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.DeleteCollaborator;

internal interface IDeleteCollaboratorCommandHandler
    : IRequestHandler<DeleteCollaboratorCommand>
{ }