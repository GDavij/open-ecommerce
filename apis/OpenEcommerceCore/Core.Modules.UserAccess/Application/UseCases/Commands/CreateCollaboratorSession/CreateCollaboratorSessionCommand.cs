using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaboratorSession;

public record CreateCollaboratorSessionCommand(
        string Email,
        string Password)
    : IRequest<ValidationResult<CreateCollaboratorSessionResponse>>;