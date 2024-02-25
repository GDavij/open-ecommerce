using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.UpdateAdministrator;

public record UpdateAdministratorCommand : IRequest<UpdateResult>
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string? Password { get; init; }
}

public record PartialUpdateAdministratorCommand(string Email, string? Password);