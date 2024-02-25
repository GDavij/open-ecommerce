using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.DeleteAdministrator;

internal interface IDeleteAdministratorCommandHandler 
    : IRequestHandler<DeleteAdministratorCommand, DeleteResult>
{ }