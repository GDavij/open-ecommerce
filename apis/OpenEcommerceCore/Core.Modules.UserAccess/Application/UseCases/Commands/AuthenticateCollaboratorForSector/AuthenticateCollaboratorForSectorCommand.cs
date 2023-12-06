using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.AuthenticateCollaboratorForSector;

public record AuthenticateCollaboratorForSectorCommand(string EncodedToken, ECollaboratorSector Sector);