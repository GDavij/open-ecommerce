using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.DeleteAdministrator;

public record DeleteAdministratorCommand(Guid Id) : IRequest<DeleteResult>;