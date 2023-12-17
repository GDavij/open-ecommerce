namespace Core.Modules.Stock.Domain.Entities;

internal class Address
{
    public Guid Id { get; init; }
    public string State { get; set; }
    public int ZipCode { get; set; }
    public string Neighbourhood { get; set; }
    public string Street { get; set; }
    
    // Relationships
    public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    
    private Address()
    {}

    public static Address Create(
        string state,
        int zipCode,
        string neighbourhood,
        string street)
    {
        return new Address
        {
            Id = Guid.NewGuid(),
            State = state,
            ZipCode = zipCode,
            Neighbourhood = neighbourhood,
            Street = street
        };
    }
}