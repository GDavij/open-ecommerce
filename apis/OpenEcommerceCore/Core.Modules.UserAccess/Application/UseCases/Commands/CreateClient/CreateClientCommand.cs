namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateClient;

public record CreateClientCommand(
    Guid ClientModuleId,
    string Email,
    string Password);