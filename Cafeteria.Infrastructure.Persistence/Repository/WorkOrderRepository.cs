using Cafeteria.Domain.Interfaces;
using Cafeteria.Domain.Model;
using Cafeteria.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Infrastructure.Persistence.Repository;

public class WorkOrderRepository : IWorkOrderRepository
{
    readonly ApplicationDbContext _context;
    public WorkOrderRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task CreateAsync(WorkOrder workOrder)
    {
        await _context.WorkOrders.AddAsync(workOrder);
        _context.SaveChanges();
    }


    public async Task<int> CountAsync()
                    => await _context.WorkOrders.CountAsync();

    public async Task<WorkOrder?> GetAsync(int id)
                    => await _context.WorkOrders.FindAsync(id);
    public User? GetUserAsync(int userId)
                    => _context.Users.Find(userId);

    public async Task<List<WorkOrderDetail>> GetPagedAsync(int pageIndex, int pageCount)
    {
        var workOrder = await _context.WorkOrders.ToListAsync();
        return await _context.WorkOrderDetails.OrderByDescending(w => w.WorkOrderId).ToListAsync();
    }

    public async Task<Kardex?> GetKardexByRawMaterialAsync(int rawMaterialId)
                    => await _context.Kardexs.FirstOrDefaultAsync(k => k.RawMaterialId == rawMaterialId);

    public void ModifyKadex(Kardex item)
    {
        if (_context.Entry(item).State == EntityState.Detached)
        {
            _context.Kardexs.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        _context.Kardexs.Update(item);
        _context.SaveChanges();
    }

    public async Task AddKadexAsync(Kardex item)
    {
        await _context.Kardexs.AddAsync(item);
        _context.SaveChanges();
    }

    public Task<List<Product>> GetProductsAsync()
    {
        var products = _context.Products.Include(p => p.ProductRawMaterials)
            .ThenInclude(prm => prm.RawMaterial)
            .ToListAsync();

        return products;
    }
    public async Task<List<Kardex>> GetKardexAsync()
    {
        return await _context.Kardexs.ToListAsync();

    }
    public async Task<List<RawMaterial>> GetRawMaterialByProductIdsAsync(int productId)
        => await _context.Products
            .Where(p => p.ProductId == productId)
            .SelectMany(p => p.ProductRawMaterials)
            .Select(prm => prm.RawMaterial)
            .ToListAsync();

    public async Task<List<Product>> GetProductByIdsAsync(List<int> productIds)
        => await _context.Products
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();

    public async Task<Taxe?> GetTaxeByUserAppIdAsync(int? userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return await _context.Taxes.FirstOrDefaultAsync(t => t.Zone == user.Zone);
    }

    public async Task CreateWorkOrderDetailsAsync(List<WorkOrderDetail> workOrderDetails)
    {
        workOrderDetails.Select(workOrderDetail => _context.WorkOrderDetails.Add(workOrderDetail));
        _context.SaveChanges();
    }

    public Product? GetProductById(int productId)
        => _context.Products.Find(productId);

    public RawMaterial GetRawMaterial(int? rawMaterialId)
            => _context.RawMaterials.Find(rawMaterialId);

    public async Task<ProductRawMaterial> GetRawMaterialsAsync(int rawMaterialId, int productId)
        => await _context.ProductRawMaterials.FirstOrDefaultAsync(p => p.RawMaterialId == rawMaterialId & p.ProductId == productId);

}
