namespace Core.Modules.HumanResources.Domain.Entities;

internal class Address
{
    public Guid Id { get; init; }
    public Collaborator Collaborator { get; init; }
    public Guid CollaboratorId { get; init; }
    public string State { get; set; }
    public int ZipCode { get; set; }
    public string Neighbourhood { get; set; }
    public string Street { get; set; }
}
