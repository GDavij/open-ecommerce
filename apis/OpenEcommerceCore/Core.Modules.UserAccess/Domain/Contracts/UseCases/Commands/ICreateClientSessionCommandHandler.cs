using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Application.UseCases.Commands.CreateClientSession;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface ICreateClientSessionCommandHandler 
    : IRequestHandler<CreateClientSessionCommand, ValidationResult<CreateClientSessionResponse>>
{ }