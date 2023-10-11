using Cafeteria.Application.Dto;
using Cafeteria.Application.Services;
using Cafeteria.Domain.Model;
using Cafeteria.Domain.ViewModel;
using System.Security.Claims;

namespace Cafeteria.Application.Interfaces;

public interface IWorkOrderService
{
    Task<IEnumerable<ListOrderViewModel>> GetWorkOrdersPage(int page, int pageSize);
    Task<OrderPrices> CreateAsync(IEnumerable<WorkOrderDetailDto> workOrderDetails, ClaimsPrincipal user);
    Task<WorkOrder?> GetByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetProductsAsync();
    Task<List<RawMaterial>> GetRawMaterialsAsync(int productId);

    Task<IEnumerable<KardexDto>> GetKardexsAsync();
}
