using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteCollaborator;

public record DeleteCollaboratorCommand(Guid Id) : IRequest;