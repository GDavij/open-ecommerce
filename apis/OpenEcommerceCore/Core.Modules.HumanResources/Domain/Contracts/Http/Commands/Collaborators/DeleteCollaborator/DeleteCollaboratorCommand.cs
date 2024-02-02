using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.DeleteCollaborator;

public record DeleteCollaboratorCommand(Guid Id) : IRequest;