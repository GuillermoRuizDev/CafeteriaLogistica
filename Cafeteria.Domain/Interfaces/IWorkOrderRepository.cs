using Cafeteria.Domain.Model;

namespace Cafeteria.Domain.Interfaces;

public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetAsync(int id);
    User? GetUserAsync(int userId);
    Task CreateAsync(WorkOrder workOrder);
    Task<int> CountAsync();
    Task<List<WorkOrderDetail>> GetPagedAsync(int pageIndex, int pageCount);
    Task<Taxe?> GetTaxeByUserAppIdAsync(int? userId);
    Task<Kardex?> GetKardexByRawMaterialAsync(int rawMaterialId);

    void ModifyKadex(Kardex item);

    Task AddKadexAsync(Kardex item);
    Task<List<Product>> GetProductsAsync();

    Task<List<Kardex>> GetKardexAsync();
    Task<List<RawMaterial>> GetRawMaterialByProductIdsAsync(int productId);
    Task<List<Product>> GetProductByIdsAsync(List<int> productIds);
    Product? GetProductById(int productId);
    Task CreateWorkOrderDetailsAsync(List<WorkOrderDetail> workOrderDetails);
    RawMaterial GetRawMaterial(int? rawMaterialId);
    Task<ProductRawMaterial> GetRawMaterialsAsync(int rawMaterialId, int productId);
}
