namespace Core.Modules.Shared.Messaging.Commands.UserAccess;

public record AuthenticateClientCommand(string EncodedToken);