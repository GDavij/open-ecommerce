using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaboratorSession;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface ICreateCollaboratorSessionCommandHandler
    : IRequestHandler<CreateCollaboratorSessionCommand, ValidationResult<CreateCollaboratorSessionResponse>>
{ }