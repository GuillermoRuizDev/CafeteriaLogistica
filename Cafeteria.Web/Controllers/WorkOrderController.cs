using Cafeteria.Application.Dto;
using Cafeteria.Application.Interfaces;
using Cafeteria.Application.Services;
using Cafeteria.Application.StaticClass;
using Cafeteria.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cafeteria.Web.Controllers;

public class WorkOrderController : Controller
{
    private readonly IWorkOrderService _workOrderService;

    public WorkOrderController(IWorkOrderService WorkOrderService)
    {
        _workOrderService = WorkOrderService;
    }


    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> Create()
    {
        var workOrderCreateViewModel = new WorkOrderCreateViewModel();

        ViewBag.Products = await _workOrderService.GetProductsAsync();

        return View(workOrderCreateViewModel);
    }


    [HttpGet]
    [Authorize(Policy = Roles.SupervisorRole)]
    public async Task<IActionResult> ListOrders()
    {
        var listOrderViewModel = new List<ListOrderViewDto>();
        listOrderViewModel = (await _workOrderService.GetWorkOrdersPage(1, 100)).ToList();
        ViewBag.Kardex = await _workOrderService.GetKardexsAsync();
        return View(listOrderViewModel);
    }

    [HttpGet]
    [Authorize(Policy = Roles.SupervisorRole)]
    public async Task<IActionResult> GetWorkOrdersPage(int page, int pageSize)
    {
        try
        {
            var workOrders = await _workOrderService.GetWorkOrdersPage(page, pageSize);

            if (workOrders == null)
            {
                return NotFound();
            }
            return Ok(workOrders);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener las ordenes de compra: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> CreateWorkOrder([FromBody] IEnumerable<WorkOrderDetailDto> products)
    {
        try
        {
            var result = await _workOrderService.CreateAsync(products, User);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al crear la orden de compra: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> GetWorkOrder(int id)
    {
        try
        {
            var workOrder = await _workOrderService.GetByIdAsync(id);

            if (workOrder == null)
            {
                return NotFound();
            }

            return Ok(workOrder);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener la orden de compra: {ex.Message}");
        }
    }

}
