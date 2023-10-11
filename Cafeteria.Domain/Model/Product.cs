namespace Cafeteria.Domain.Model;

public class Product
{
    public Product()
    {
        this.ProductRawMaterials = new HashSet<ProductRawMaterial>();
        this.WorkOrderDetails = new HashSet<WorkOrderDetail>();
    }

    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ICollection<ProductRawMaterial> ProductRawMaterials { get; set; }

    public ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }
}