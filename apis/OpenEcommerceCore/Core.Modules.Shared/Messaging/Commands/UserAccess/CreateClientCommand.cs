namespace Core.Modules.Shared.Messaging.Commands.UserAccess;

public record CreateClientCommand(
    Guid ClientModuleId,
    string Email,
    string Password);