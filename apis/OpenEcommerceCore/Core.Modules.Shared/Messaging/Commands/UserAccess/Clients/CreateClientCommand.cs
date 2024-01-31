namespace Core.Modules.Shared.Messaging.Commands.UserAccess.Clients;

public record CreateClientCommand(
    Guid ClientModuleId,
    string Email,
    string Password);