using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class Supplier
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public List<Product> Products { get; set; }
    public Address Address { get; set; }
    public int SalesNumber { get; set; }
    
    private Supplier(
        Guid id,
        string name,
        string email,
        string phone,
        List<Product> products,
        Address address,
        int salesNumber)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        SalesNumber = salesNumber;
        Products = products;
    }

    public Supplier Create(
        string name,
        string email,
        string phone,
        List<Product> products,
        Address address,
        int salesNumber = 0)
    {
        return new Supplier(
            Guid.NewGuid(),
            name,
            email,
            phone,
            products,
            address,
            salesNumber);
    }
}