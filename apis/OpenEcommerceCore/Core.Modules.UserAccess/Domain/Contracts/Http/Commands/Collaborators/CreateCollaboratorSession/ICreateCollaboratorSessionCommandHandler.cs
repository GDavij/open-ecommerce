using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Collaborators.CreateCollaboratorSession;

public interface ICreateCollaboratorSessionCommandHandler
    : IRequestHandler<CreateCollaboratorSessionCommand, ValidationResult<CreateCollaboratorSessionResponse>>
{ }