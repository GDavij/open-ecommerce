using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.UpdateAdministrator;

internal interface IUpdateAdministratorCommandHandler
    : IRequestHandler<UpdateAdministratorCommand, UpdateResult>
{ }