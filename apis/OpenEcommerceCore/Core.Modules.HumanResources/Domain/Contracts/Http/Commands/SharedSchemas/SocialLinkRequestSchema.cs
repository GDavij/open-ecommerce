using Core.Modules.HumanResources.Domain.Enums;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;

public record SocialLinkRequestSchema
{
    public SocialMedia SocialMedia { get; init; }
    public string Url { get; init; }
}
