namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SharedSchemas;

public record AddressRequestSchema
{
    public Guid StateId { get; init; }
    public int ZipCode { get; init; }
    public string Neighbourhood { get; init; }
    public string Street { get; init; }
}