using Core.Modules.Stock.Domain.Entities;
using Core.Modules.Stock.Domain.Entities.Complex.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Domain.Contracts.Contexts;

internal interface IStockContext
{
        DbSet<Brand> Brands { get; set; }
        DbSet<Collaborator> Collaborators { get; set; }
        DbSet<Distributor> Distributors { get; set; }
        DbSet<MeasureUnit> MeasureUnits { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductRestockOrder> ProductRestockOrders { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}