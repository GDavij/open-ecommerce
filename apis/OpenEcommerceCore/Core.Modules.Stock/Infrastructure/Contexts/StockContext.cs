using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Complex.Product;
using Core.Modules.Stock.Domain.Entities.Demands;
using Core.Modules.Stock.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Infrastructure.Contexts;

internal class StockContext 
    : DbContext, IStockContext
{
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<Supplier> Distributors { get; set; }
    public DbSet<MeasureUnit> MeasureUnits { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductRestockDemand> ProductRestockOrders { get; set; }
    public DbSet<Address> Addresses { get; set; }

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
        modelBuilder.ApplyConfiguration(new ProductRestockDemandMappings());
        modelBuilder.ApplyConfiguration(new ProductTagMappings());
        modelBuilder.ApplyConfiguration(new SupplierMappings());
        modelBuilder.ApplyConfiguration(new AddressMappings());
    }
}