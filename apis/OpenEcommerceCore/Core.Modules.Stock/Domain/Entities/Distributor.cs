namespace Core.Modules.Stock.Domain.Entities;

internal class Distributor
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public Address Address { get; set; }
    public int SalesNumber { get; set; }
    
    private Distributor(Guid id, string name,  string email, string phone, Address address, int salesNumber)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        SalesNumber = salesNumber;
    }

    public Distributor Create(Guid id, string name, string email, string phone,Address address, int salesNumber = 0)
    {
        return new Distributor(
            id,
            name,
            email,
            phone,
            address,
            salesNumber);
    }
}

internal class Address
{
    public string State { get; set; }
    public int ZipCode { get; set; }
    public string Neighbourhood { get; set; }
    public string Street { get; set; }

    private Address(
        string state,
        int zipCode,
        string neighbourhood,
        string street)
    {
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
            state,
            zipCode,
            neighbourhood,
            street
        );
    }
}