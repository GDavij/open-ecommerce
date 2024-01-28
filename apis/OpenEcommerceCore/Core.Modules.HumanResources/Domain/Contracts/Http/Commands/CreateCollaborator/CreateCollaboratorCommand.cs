using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Enums;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;

public record CreateCollaboratorCommand : IRequest<CreateCollaboratorCommandResponse>
{
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Description { get; init; }
    public int Age { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string Password { get; init; }
    public List<ContractRequestSchema> Contracts { get; init; }
    public List<SocialLinkRequestSchema> SocialLinks { get; init; } 
    public List<AddressRequestSchema> Addresses { get; init; }
};



