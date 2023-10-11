//using Cafeteria.Domain.Model;
//using Cafeteria.Infrastructure.Persistence.Context;
//using Microsoft.Extensions.DependencyInjection;

//namespace Cafeteria.Infrastructure.Persistence;

//public static class SeedData
//{
//    public static async Task InitializeAsync(
//        IServiceProvider services)
//    {
//        using (var scope = services.CreateScope())
//        {
//            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

//            var rawMaterials = GenerateRawMaterialData();
//            db.RawMaterials.AddRangeAsync(rawMaterials);
//            db.SaveChanges();

//            var products = GenerateProductData();
//            db.Products.AddRangeAsync(products);
//            db.SaveChanges();

//            var productsRawMaterials = GenerateProductRawMaterialData();
//            db.ProductRawMaterials.AddRangeAsync(productsRawMaterials);
//            db.SaveChanges();

//            var users = GenerateUserData();
//            db.Users.AddRangeAsync(users);
//            db.SaveChanges();

//            var workOrders = GenerateWorkOrderData();
//            db.WorkOrders.AddRangeAsync(workOrders);
//            db.SaveChanges();

//            var kardexs = GenerateKardexData();
//            db.Kardexs.AddRangeAsync(kardexs);
//            db.SaveChanges();
//        }
//    }

//    private static List<ProductRawMaterial> GenerateProductRawMaterialData()
//    {
//        throw new NotImplementedException();
//    }

//    public static List<Kardex> GenerateKardexData()
//    {
//        var kardexData = new List<Kardex>();

//        // Generar datos ficticios para Kardex
//        var random = new Random();
//        var transactionDates = GenerateRandomDates(DateTime.Now.AddYears(-2), DateTime.Now, 100);
//        var descriptions = new List<string>
//    {
//        "Compra de café",
//        "Venta de latte",
//        "Ajuste de inventario",
//        "Venta de pastel",
//        "Compra de leche",
//        "Ajuste de stock de tazas",
//    };

//        foreach (var date in transactionDates)
//        {
//            var kardexItem = new Kardex
//            {
//                TransactionDate = date,
//                Description = descriptions[random.Next(descriptions.Count)],
//                Quantity = random.Next(1, 20),
//                ProductId = random.Next(1, 10),
//                RawMaterialId = random.Next(1, 10)
//            };
//            kardexData.Add(kardexItem);
//        }

//        return kardexData;
//    }

//    public static List<Product> GenerateProductData()
//    {
//        var productData = new List<Product>();

//        // Generar datos ficticios para Product
//        var productNames = new List<string>
//    {
//        "Café Americano",
//        "Café Latte",
//        "Café Espresso",
//        "Tarta de Manzana",
//        "Croissant de Chocolate",
//        "Muffin de Arándanos",
//    };

//        var random = new Random();

//        foreach (var productName in productNames)
//        {
//            var productItem = new Product
//            {
//                Name = productName,
//                Price = random.Next(2, 10) * 2.5m // Precios aleatorios en intervalo de 5 en 5
//            };
//            productData.Add(productItem);
//        }

//        return productData;
//    }

//    public static List<RawMaterial> GenerateRawMaterialData()
//    {
//        var rawMaterialData = new List<RawMaterial>();

//        // Generar datos ficticios para RawMaterial
//        var materialNames = new List<string>
//    {
//        "Café en Grano",
//        "Leche",
//        "Azúcar",
//        "Harina",
//        "Huevos",
//        "Chocolate",
//    };

//        var random = new Random();

//        foreach (var materialName in materialNames)
//        {
//            var rawMaterialItem = new RawMaterial
//            {
//                Name = materialName,
//                AvailableQuantity = random.Next(100, 500) // Cantidades aleatorias entre 100 y 500
//            };
//            rawMaterialData.Add(rawMaterialItem);
//        }

//        return rawMaterialData;
//    }

//    public static List<User> GenerateUserData()
//    {
//        var userData = new List<User>();

//        // Generar datos ficticios para User
//        var userNames = new List<string>
//    {
//        "Usuario1",
//        "Empleado1",
//        "Supervisor1",
//        "Administrador1"
//    };

//        foreach (var userName in userNames)
//        {
//            var userItem = new User
//            {
//                Name = userName,
//                Email = $"{userName}@cafeteria.com",
//                Role = GetRoleFromUserName(userName)
//            };
//            userData.Add(userItem);
//        }

//        return userData;
//    }

//    public static List<WorkOrder> GenerateWorkOrderData()
//    {
//        var workOrderData = new List<WorkOrder>();

//        // Generar datos ficticios para WorkOrder
//        var random = new Random();
//        var creationDates = GenerateRandomDates(DateTime.Now.AddYears(-2), DateTime.Now, 50);
//        var statuses = new List<string> { "En progreso", "Completada", "Cancelada" };

//        foreach (var date in creationDates)
//        {
//            var workOrderItem = new WorkOrder
//            {
//                CreationDate = date,
//                Status = statuses[random.Next(statuses.Count)]
//            };
//            workOrderData.Add(workOrderItem);
//        }

//        return workOrderData;
//    }

//    private static List<DateTime> GenerateRandomDates(DateTime startDate, DateTime endDate, int count)
//    {
//        var random = new Random();
//        var dateList = new List<DateTime>();
//        for (var i = 0; i < count; i++)
//        {
//            var randomTicks = (long)(random.NextDouble() * (endDate.Ticks - startDate.Ticks)) + startDate.Ticks;
//            dateList.Add(new DateTime(randomTicks));
//        }
//        return dateList;
//    }

//    private static string GetRoleFromUserName(string userName)
//    {
//        if (userName.Contains("Usuario")) return "Usuario";
//        if (userName.Contains("Empleado")) return "Empleado";
//        if (userName.Contains("Supervisor")) return "Supervisor";
//        if (userName.Contains("Administrador")) return "Administrador";
//        return "Usuario";
//    }

//}
