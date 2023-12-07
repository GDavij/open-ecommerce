using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaborator;

internal record CreateCollaboratorCommand(
    Guid CollaboratorModuleId,
    ECollaboratorSector CollaboratorSector,
    string Email,
    string Password);