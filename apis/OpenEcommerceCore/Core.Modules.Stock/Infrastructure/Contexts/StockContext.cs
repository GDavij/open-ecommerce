using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Demands;
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Core.Modules.Stock.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Infrastructure.Contexts;

internal class StockContext
    : DbContext, IStockContext
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<MeasureUnit> MeasureUnits { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<MeasurementDetail> Products_MeasureDetails { get; set; }
    public DbSet<TechnicalDetail> Products_TechnicalDetails { get; set; }
    public DbSet<OtherDetail> Products_OtherDetails { get; set; }
    public DbSet<ProductRestockDemand> ProductRestockOrders { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = Environment.GetEnvironmentVariable("STOCK_POSTGRES_DATABASE")!;
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BrandMappings());
        modelBuilder.ApplyConfiguration(new CollaboratorMappings());
        modelBuilder.ApplyConfiguration(new DemandMessageMappings());
        modelBuilder.ApplyConfiguration(new MeasureUnitMappings());
        modelBuilder.ApplyConfiguration(new ProductImageMappings());
        modelBuilder.ApplyConfiguration(new ProductMappings());
        modelBuilder.ApplyConfiguration(new Product_MeasureDetailMappings());
        modelBuilder.ApplyConfiguration(new Product_TechnicalDetailMappings());
        modelBuilder.ApplyConfiguration(new Product_OtherDetailMappings());
        modelBuilder.ApplyConfiguration(new ProductRestockDemandMappings());
        modelBuilder.ApplyConfiguration(new ProductTagMappings());
        modelBuilder.ApplyConfiguration(new SupplierMappings());
        modelBuilder.ApplyConfiguration(new AddressMappings());
    }
}