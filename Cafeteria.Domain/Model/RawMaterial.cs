namespace Cafeteria.Domain.Model;

public class RawMaterial
{
    public RawMaterial()
    {
        this.ProductRawMaterials = new HashSet<ProductRawMaterial>();
        this.Kardexs = new HashSet<Kardex>();

    }
    public int RawMaterialId { get; set; }
    public string Name { get; set; }
    public int AvailableQuantity { get; set; }
    public ICollection<ProductRawMaterial> ProductRawMaterials { get; set; }
    public ICollection<Kardex> Kardexs { get; set; }
}
