using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Clients.CreateClientSession;

public record CreateClientSessionCommand(
        string Email,
        string Password)
    : IRequest<ValidationResult<CreateClientSessionResponse>>;