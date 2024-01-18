using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Application.Http.Commands.CreateCollaboratorSession;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands;

public interface ICreateCollaboratorSessionCommandHandler
    : IRequestHandler<CreateCollaboratorSessionCommand, ValidationResult<CreateCollaboratorSessionResponse>>
{ }