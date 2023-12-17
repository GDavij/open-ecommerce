using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class Supplier
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public Address Address { get; set; }
    public int SalesNumber { get; set; }
    
    // Relationships
    public List<Product> Products { get; set; } = new List<Product>();
    
    private Supplier()
    {}

    public static Supplier Create(
        Guid id,
        string name,
        string email,
        string phone,
        Address address,
        int salesNumber = 0)
    {
        return new Supplier
        {
            Id = id,
            Name = name,
            Email = email,
            Phone = phone,
            Address = address,
            SalesNumber = salesNumber
        };
    }
}