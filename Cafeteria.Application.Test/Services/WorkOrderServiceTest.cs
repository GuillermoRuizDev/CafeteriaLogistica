using Cafeteria.Application.Dto;
using Cafeteria.Application.Services;
using Cafeteria.Domain.Interfaces;
using Cafeteria.Domain.Model;
using Cafeteria.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

namespace Cafeteria.Application.Test.Services;

[TestFixture]
public class WorkOrderServiceTests
{
    private Mock<IWorkOrderRepository> _workOrderRepository;
    private Mock<UserManager<ApplicationUser>> _userManager;
    private WorkOrderService _workOrderService;

    [SetUp]
    public void SetUp()
    {
        var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BotTest")
                .Options;

        _userManager = new Mock<UserManager<ApplicationUser>>(
            new Mock<IUserStore<ApplicationUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new IUserValidator<ApplicationUser>[0],
            new IPasswordValidator<ApplicationUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        //ApplicationDbContext dbContext = new ApplicationDbContext(option);
        _workOrderRepository = new Mock<IWorkOrderRepository>();
        //_userManager = new Mock<UserManager<ApplicationUser>>(/* configuración, proveedor, etc. */);

        _workOrderService = new WorkOrderService(_workOrderRepository.Object, _userManager.Object);
    }

    [Test]
    public async Task InitClassWorkOrderServer()
    {
        //Arrange
        var workOrderService = new WorkOrderService(_workOrderRepository.Object, _userManager.Object);

        Assert.IsNotNull(workOrderService);
    }

    // Prueba para GetByIdAsync
    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsWorkOrder()
    {
        // Arrange
        int orderId = 1;
        var expectedWorkOrder = new WorkOrder { WorkOrderId = orderId };
        _workOrderRepository.Setup(r => r.GetAsync(orderId)).ReturnsAsync(expectedWorkOrder);

        // Act
        var result = await _workOrderService.GetByIdAsync(orderId);

        // Assert
        Assert.AreEqual(expectedWorkOrder, result);
    }

    // Prueba para CreateAsync
    [Test]
    public async Task CreateAsync_WithValidData_CreatesWorkOrderAndDetails()
    {
        // Arrange
        var workOrderDetails = new List<WorkOrderDetailDto>
        {
            new WorkOrderDetailDto(Id:0,WorkOrderId:0, ProductId : 1, Quantity : 2 , Price:0),
            new WorkOrderDetailDto(Id:0,WorkOrderId:0, ProductId : 2, Quantity : 1 , Price:0)
        };
        var user = new ClaimsPrincipal(); // Simula un usuario autenticado

        var expectedUser = new ApplicationUser { UserId = 1 };
        var expectedTaxe = new Taxe { Value = 0.1M }; // Supongamos un valor de impuesto del 10%
        var expectedProducts = new List<Product>
        {
            new Product { ProductId = 1, Price = 10.99m },
            new Product { ProductId = 2, Price = 19.99m }
        };

        _userManager.Setup(u => u.GetUserAsync(user)).ReturnsAsync(expectedUser);
        _workOrderRepository.Setup(r => r.GetTaxeByUserAppIdAsync(expectedUser.UserId)).ReturnsAsync(expectedTaxe);
        _workOrderRepository.Setup(r => r.GetProductByIdsAsync(workOrderDetails.Select(wd => wd.ProductId).ToList()))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _workOrderService.CreateAsync(workOrderDetails, user);

        // Assert
        Assert.IsNotNull(result);

        // Verifica que se haya llamado a _workOrderRepository.CreateAsync con los argumentos correctos
        _workOrderRepository.Verify(r => r.CreateAsync(It.Is<WorkOrder>(wo =>
            wo.UserId == expectedUser.UserId &&
            wo.Status == "Pendiente" && // Asegúrate de que otros campos se configuren correctamente
            wo.WorkOrderDetails != null &&
            wo.WorkOrderDetails.Count == expectedProducts.Count
        )), Times.Once);

        // Verifica que se hayan llamado a _workOrderRepository.CreateWorkOrderDetailsAsync para cada detalle
        foreach (var product in expectedProducts)
        {
            _workOrderRepository.Verify(r => r.CreateWorkOrderDetailsAsync(It.Is<List<WorkOrderDetail>>(details =>
                details.Count == 1 && // Debe haber un detalle por producto
                details[0].ProductId == product.ProductId &&
                details[0].Quantity == workOrderDetails.First(wd => wd.ProductId == product.ProductId).Quantity &&
                details[0].Price == product.Price * (decimal)workOrderDetails.First(wd => wd.ProductId == product.ProductId).Quantity
            )), Times.Once);
        }

        // Agrega más aserciones según sea necesario
    }
}