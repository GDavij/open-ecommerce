using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Demands;
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Core.Modules.Stock.Domain.Contracts.Contexts;

internal interface IStockContext
{
        DbSet<Brand> Brands { get; set; }
        DbSet<Collaborator> Collaborators { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<MeasureUnit> MeasureUnits { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<MeasurementDetail> Products_MeasureDetails { get; set; }
        DbSet<TechnicalDetail> Products_TechnicalDetails { get; set; }
        DbSet<OtherDetail> Products_OtherDetails { get; set; }
        DbSet<ProductRestockDemand> ProductRestockOrders { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<ProductTag> ProductTags { get; set; }
        DbSet<ProductImage> ProductImages { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}