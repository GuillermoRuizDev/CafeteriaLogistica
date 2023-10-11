using Cafeteria.Domain.Model;
using Cafeteria.Infrastructure.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Cafeteria.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(
        IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            GenerateProductData(db);

            GenerateUserData(db);
        }
    }

    public static void GenerateProductData(ApplicationDbContext db)
    {
        var productData = new List<Product>();

        var latte = new Product()
        {
            ProductId = 1,
            Name = "Latte",
            Price = 3.50M,
        };
        productData.Add(latte);

        var carrotCake = new Product()
        {
            ProductId = 2,
            Name = "Carrot Cake",
            Price = 2.50M,
        };
        productData.Add(carrotCake);

        var freshMilk = new Product()
        {
            ProductId = 3,
            Name = "Fresh Milk",
            Price = 1.50M,
        };
        productData.Add(freshMilk);

        var coffee = new Product()
        {
            ProductId = 4,
            Name = "Coffee",
            Price = 1.00M,
        };
        productData.Add(coffee);

        var cafeConLeche = new Product()
        {
            ProductId = 5,
            Name = "Café con Leche",
            Price = 2.00M,
        };
        productData.Add(cafeConLeche);

        db.Products.AddRangeAsync(productData);
        db.SaveChanges();


        var rawMaterialData = new List<RawMaterial>();

        var coffeeRawMaterial = new RawMaterial()
        {
            RawMaterialId = 1,
            Name = "Coffee",
            AvailableQuantity = 100,
        };
        rawMaterialData.Add(coffeeRawMaterial);

        var freshMilkRawMaterial = new RawMaterial()
        {
            RawMaterialId = 2,
            Name = "Fresh Milk",
            AvailableQuantity = 200,
        };
        rawMaterialData.Add(freshMilkRawMaterial);

        db.RawMaterials.AddRangeAsync(rawMaterialData);
        db.SaveChanges();

        var productRawMaterials = new List<ProductRawMaterial>();

        productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 1,
            ProductId = latte.ProductId,
            RawMaterialId = coffeeRawMaterial.RawMaterialId,
            Quantity = 1,
        });

        productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 2,
            ProductId = latte.ProductId,
            RawMaterialId = freshMilkRawMaterial.RawMaterialId,
            Quantity = 2,
        });

        productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 3,
            ProductId = carrotCake.ProductId,
            RawMaterialId = freshMilkRawMaterial.RawMaterialId,
            Quantity = 1,
        });



        productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 4,
            ProductId = cafeConLeche.ProductId,
            RawMaterialId = coffeeRawMaterial.RawMaterialId,
            Quantity = 5,
        });
        productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 5,
            ProductId = cafeConLeche.ProductId,
            RawMaterialId = freshMilkRawMaterial.RawMaterialId,
            Quantity = 1,
        });

        productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 6,
            ProductId = coffee.ProductId,
            RawMaterialId = freshMilkRawMaterial.RawMaterialId,
            Quantity = 1,
        }); productRawMaterials.Add(new ProductRawMaterial()
        {
            ProductRawMaterialId = 7,
            ProductId = freshMilk.ProductId,
            RawMaterialId = freshMilkRawMaterial.RawMaterialId,
            Quantity = 1,
        });

        db.ProductRawMaterials.AddRangeAsync(productRawMaterials);
        db.SaveChanges();


        var kardexData = new List<Kardex>();

        // Kardex
        var latteKardex = new Kardex()
        {
            TransactionDate = DateTime.Now,
            Description = "Inventario inicial",
            Quantity = 50,
            RawMaterialId = 1,
        };
        kardexData.Add(latteKardex);

        var carrotCakeKardex = new Kardex()
        {
            TransactionDate = DateTime.Now,
            Description = "Inventario inicial",
            Quantity = 25,
            RawMaterialId = 2,
        };
        kardexData.Add(carrotCakeKardex);



        db.Kardexs.AddRangeAsync(kardexData);
        db.SaveChanges();


        var workOrderData = new List<WorkOrder>();

        var order1 = new WorkOrder()
        {
            WorkOrderId = 1,
            UserId = 1,
            CreationDate = DateTime.Now,
            Status = "Pendiente"

        };
        workOrderData.Add(order1);

        // Pedido 2
        var order2 = new WorkOrder()
        {
            WorkOrderId = 2,
            UserId = 2,
            CreationDate = DateTime.Now,
            Status = "Pagado"
        };
        workOrderData.Add(order2);

        // Pedido 3
        var order3 = new WorkOrder()
        {
            WorkOrderId = 3,
            UserId = 1,
            CreationDate = DateTime.Now,
            Status = "Entregado"
        };
        workOrderData.Add(order3);

        db.WorkOrders.AddRange(workOrderData);
        db.SaveChanges();



        var workOrderDetails = new List<WorkOrderDetail>();

        workOrderDetails.Add(new WorkOrderDetail()
        {
            WorkOrderDetailId = 1,
            WorkOrderId = order1.WorkOrderId,
            Quantity = 2,
            Price = 10.99m, // Precio del producto 1
            ProductId = latte.ProductId
        });

        workOrderDetails.Add(new WorkOrderDetail()
        {
            WorkOrderDetailId = 2,
            WorkOrderId = order1.WorkOrderId,
            Quantity = 1,
            Price = 19.99m, // Precio del producto 2
            ProductId = carrotCake.ProductId
        });


        workOrderDetails.Add(new WorkOrderDetail()
        {
            WorkOrderDetailId = 3,
            WorkOrderId = order2.WorkOrderId,
            Quantity = 5,
            Price = 7.99m, // Precio del producto 3
            ProductId = freshMilk.ProductId
        });
        workOrderDetails.Add(new WorkOrderDetail()
        {
            WorkOrderDetailId = 4,
            WorkOrderId = order2.WorkOrderId,
            Quantity = 1,
            Price = 29.99m, // Precio del producto 4
            ProductId = coffee.ProductId
        });

        workOrderDetails.Add(new WorkOrderDetail()
        {
            WorkOrderDetailId = 5,
            WorkOrderId = order3.WorkOrderId,
            Quantity = 3,
            Price = 14.99m, // Precio del producto 5
            ProductId = cafeConLeche.ProductId
        });

        db.WorkOrderDetails.AddRange(workOrderDetails);
        db.SaveChanges();
    }

    public static void GenerateUserData(ApplicationDbContext db)
    {
        var userData = new List<User>();

        userData.Add(new User()
        {
            UserId = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Role = "Usuario",
            Zone = "Lima",
        });

        userData.Add(new User()
        {
            UserId = 2,
            Name = "Luis Fernandez",
            Email = "luis.fernandez@example.com",
            Role = "Usuario",
            Zone = "Lima",
        });

        userData.AddRange(userData);

        db.Users.AddRangeAsync(userData);
        db.SaveChanges();


        var zoneData = new List<Taxe>();

        zoneData.Add(new Taxe()
        {
            TaxeId = 1,
            Zone = "Lima",
            Value = 0.18M
        });

        zoneData.Add(new Taxe()
        {
            TaxeId = 2,
            Zone = "Buenos aires",
            Value = 0.12M
        });


        db.Taxes.AddRangeAsync(zoneData);
        db.SaveChanges();
    }



}
