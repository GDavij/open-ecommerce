namespace Core.Modules.HumanResources.Domain.Exceptions.Collaborators;

internal class AlreadyDeletedCollaboratorException : Exception
{
    public AlreadyDeletedCollaboratorException(Guid collaboratorId) : base($"Found a already deleted collaborator with id {collaboratorId}")
    {}
}