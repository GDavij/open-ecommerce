using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Collaborators.CreateCollaboratorSession;

public record CreateCollaboratorSessionCommand(
        string Email,
        string Password)
    : IRequest<ValidationResult<CreateCollaboratorSessionResponse>>;