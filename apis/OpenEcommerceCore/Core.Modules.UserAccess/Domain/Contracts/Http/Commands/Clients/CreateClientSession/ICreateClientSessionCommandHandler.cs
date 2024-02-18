using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Clients.CreateClientSession;

public interface ICreateClientSessionCommandHandler 
    : IRequestHandler<CreateClientSessionCommand, ValidationResult<CreateClientSessionResponse>>
{ }