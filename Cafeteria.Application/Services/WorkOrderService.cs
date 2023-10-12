using Cafeteria.Application.Dto;
using Cafeteria.Application.Interfaces;
using Cafeteria.Domain.Interfaces;
using Cafeteria.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Cafeteria.Application.Services;

public class WorkOrderService : IWorkOrderService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWorkOrderRepository _workOrderRepository;

    public WorkOrderService(IWorkOrderRepository workOrderRepository, UserManager<ApplicationUser> userManager)
    {
        _workOrderRepository = workOrderRepository;
        _userManager = userManager;
    }
    public async Task<WorkOrder?> GetByIdAsync(int id)
    {
        return await _workOrderRepository.GetAsync(id);
    }

    public async Task<List<ListOrderViewDto>> GetWorkOrdersPage(int page, int pageSize)
    {
        int totalItems = await _workOrderRepository.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var workOrder = await _workOrderRepository.GetPagedAsync(
            page,
            totalPages
            );


        var result = workOrder.Select(wo => new ListOrderViewDto(

            User: _workOrderRepository.GetUserAsync(wo.WorkOrder.UserId).Name,
            Product: _workOrderRepository.GetProductById(wo.ProductId).Name,
            CreationDate: String.Format("{0:d}", wo.WorkOrder.CreationDate),
            Quantity: wo.Quantity,
            Status: wo.WorkOrder.Status
        ));

        return result.ToList();
    }

    public async Task<IEnumerable<KardexDto>> GetKardexsAsync()
    {
        return (await _workOrderRepository.GetKardexAsync()).Select(kardex => new
        KardexDto(
            KardexId: kardex.KardexId,
             TransactionDate: String.Format("{0:d}", kardex.TransactionDate),
             Description: kardex.Description,
             Quantity: kardex.Quantity,
             NameProduct: _workOrderRepository.GetRawMaterial(kardex.RawMaterialId).Name
        ));
    }

    public async Task<OrderPrices> CreateAsync(IEnumerable<WorkOrderDetailDto> workOrderDetails, ClaimsPrincipal user)
    {
        var userApp = await _userManager.GetUserAsync(user);
        var taxe = await _workOrderRepository.GetTaxeByUserAppIdAsync(userApp.UserId);
        var products = await _workOrderRepository.GetProductByIdsAsync(workOrderDetails.Select(p => p.ProductId).ToList());

        decimal subtotal = products.
            Sum(p => p.Price * (decimal)workOrderDetails.Where(wd => wd.ProductId == p.ProductId).Sum(w => w.Quantity));
        decimal taxeValue = taxe != null ? taxe.Value : 0;
        decimal taxeTotal = subtotal * taxeValue;

        decimal total = subtotal + taxeTotal;

        WorkOrder workOrder = new WorkOrder
        {
            UserId = (int)userApp?.UserId,
            CreationDate = DateTime.Now,
            Status = "Pendiente",
            SubTotal = subtotal,
            Taxe = taxeTotal,
            Total = total,
            WorkOrderDetails = products.Select(product => new WorkOrderDetail
            {
                ProductId = product.ProductId,
                Quantity = workOrderDetails.Where(w => w.ProductId == product.ProductId).Sum(p => p.Quantity),
                Price = product.Price * (decimal)workOrderDetails.Where(w => w.ProductId == product.ProductId).FirstOrDefault().Quantity
            }).ToList()
        };

        await _workOrderRepository.CreateAsync(workOrder);

        await _workOrderRepository.CreateWorkOrderDetailsAsync(workOrder.WorkOrderDetails.ToList());

        foreach (var orderDetail in workOrder.WorkOrderDetails)
        {
            await UpdateKardex(orderDetail.Product, orderDetail.Quantity);
        }

        return new OrderPrices(subtotal, taxeTotal, total);
    }

    private async Task UpdateKardex(Product product, double quantity)
    {
        var rawMaterials = await _workOrderRepository.GetRawMaterialByProductIdsAsync(product.ProductId);

        foreach (var rawMaterial in rawMaterials)
        {
            var kardex = await _workOrderRepository.GetKardexByRawMaterialAsync(rawMaterial.RawMaterialId);
            var productRawMaterial = await _workOrderRepository.GetRawMaterialsAsync(rawMaterial.RawMaterialId, product.ProductId);

            if (kardex == null)
                return;

            kardex.TransactionDate = DateTime.Now;
            kardex.Description = $"{rawMaterial.Name}";
            kardex.Quantity = kardex.Quantity - (productRawMaterial.Quantity * (decimal)quantity);
            kardex.RawMaterialId = rawMaterial.RawMaterialId;

            _workOrderRepository.ModifyKadex(kardex);
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        return (await _workOrderRepository.GetProductsAsync()).ToList().Select(product => new
        ProductDto(
            ProductId: product.ProductId,
            Name: product.Name,
            Price: product.Price
        ));
    }

    public async Task<List<RawMaterial>> GetRawMaterialsAsync(int productId)
    {
        return await _workOrderRepository.GetRawMaterialByProductIdsAsync(productId);
    }

}
