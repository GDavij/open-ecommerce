namespace Core.Modules.Shared.Messaging.Commands.UserAccess.Auth;

public record AuthenticateClientCommand(string EncodedToken);