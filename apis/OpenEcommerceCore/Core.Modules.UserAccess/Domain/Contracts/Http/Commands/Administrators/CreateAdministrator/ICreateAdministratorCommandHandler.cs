using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.CreateAdministrator;

internal interface ICreateAdministratorCommandHandler
    : IRequestHandler<CreateAdministratorCommand, CreationResult>
{ }