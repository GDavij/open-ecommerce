using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateClientSession;

public record CreateClientSessionCommand(
        string Email,
        string Password)
    : IRequest<ValidationResult<CreateClientSessionResponse>>;