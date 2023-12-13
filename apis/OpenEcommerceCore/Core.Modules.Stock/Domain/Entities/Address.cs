namespace Core.Modules.Stock.Domain.Entities;

internal class Address
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public int ZipCode { get; set; }
    public string Neighbourhood { get; set; }
    public string Street { get; set; }
    
    private Address(
        Guid guid,
        string state,
        int zipCode,
        string neighbourhood,
        string street)
    {
        Id = guid;
        State = state;
        ZipCode = zipCode;
        Neighbourhood = neighbourhood;
        Street = street;
    }

    public Address Create(
        string state,
        int zipCode,
        string neighbourhood,
        string street)
    {
        return new Address(
            Guid.NewGuid(),
            state,
            zipCode,
            neighbourhood,
            street
        );
    }
}