using Core.Modules.Shared.Domain.ResultObjects;
using MediatR;

namespace Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.CreateAdministrator;

public record CreateAdministratorCommand : IRequest<CreationResult>
{
    public string Email { get; init; } 
    public string Password { get; init; } 
    public string? Authorization { get; init; }  
}

public record PartialCreateAdministratorCommand 
{
    public string Email { get; init; }
    public string Password { get; init; }
}