using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Application.Http.Commands.CreateClientSession;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands;

public interface ICreateClientSessionCommandHandler 
    : IRequestHandler<CreateClientSessionCommand, ValidationResult<CreateClientSessionResponse>>
{ }