using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.SendJobApplication;

public record SendJobApplicationCommand : IRequest
{
    public string FirstName { get; init; }
    public string? LastName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public int Age { get; init; }
    public ECollaboratorSector Sector { get; init; }
    public IFormFile Resume { get; init; }
    public List<SocialLinkRequestSchema> SocialLinks { get; init; }
}