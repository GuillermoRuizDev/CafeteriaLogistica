using Cafeteria.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Infrastructure.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RawMaterial> RawMaterials { get; set; }
    public DbSet<ProductRawMaterial> ProductRawMaterials { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WorkOrderDetail> WorkOrderDetails { get; set; }
    public DbSet<Kardex> Kardexs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Taxe> Taxes { get; set; }

}