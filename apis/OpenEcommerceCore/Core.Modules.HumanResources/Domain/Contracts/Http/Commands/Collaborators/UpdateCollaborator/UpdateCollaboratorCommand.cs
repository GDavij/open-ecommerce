using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.UpdateCollaborator;

public record UpdateCollaboratorCommand : IRequest<UpdateCollaboratorCommandResponse>
{
    public Guid Id {get; init; } 
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Description { get; init; }
    public int Age { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string? Password { get; init; }
    public List<SocialLinkRequestSchema> SocialLinks { get; init; } 
    public List<AddressRequestSchema> Addresses { get; init; }
}