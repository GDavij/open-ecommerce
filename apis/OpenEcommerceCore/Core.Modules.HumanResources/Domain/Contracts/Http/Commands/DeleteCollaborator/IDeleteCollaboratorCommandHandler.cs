using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteCollaborator;

internal interface IDeleteCollaboratorCommandHandler
    : IRequestHandler<DeleteCollaboratorCommand>
{ }