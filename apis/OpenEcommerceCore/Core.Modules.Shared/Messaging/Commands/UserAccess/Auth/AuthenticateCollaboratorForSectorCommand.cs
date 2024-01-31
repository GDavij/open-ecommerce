using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.Shared.Messaging.Commands.UserAccess.Auth;

public record AuthenticateCollaboratorForSectorCommand(string EncodedToken, ECollaboratorSector Sector);