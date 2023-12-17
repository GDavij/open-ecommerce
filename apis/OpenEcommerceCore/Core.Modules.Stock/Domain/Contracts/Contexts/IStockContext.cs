using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Demands;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Domain.Contracts.Contexts;

internal interface IStockContext
{
        DbSet<Brand> Brands { get; set; }
        DbSet<Collaborator> Collaborators { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<MeasureUnit> MeasureUnits { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductRestockDemand> ProductRestockOrders { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<ProductTag> ProductTags { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}