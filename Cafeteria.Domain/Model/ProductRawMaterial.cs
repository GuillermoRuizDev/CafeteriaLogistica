namespace Cafeteria.Domain.Model;

public class ProductRawMaterial
{
    public int ProductRawMaterialId { get; set; }
    public int ProductId { get; set; }
    public int RawMaterialId { get; set; }
    public int Quantity { get; set; }
    public virtual Product Product { get; set; }
    public virtual RawMaterial RawMaterial { get; set; }
}
