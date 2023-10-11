namespace Cafeteria.Domain.Model;

public class Kardex
{
    public int KardexId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; }
    public decimal Quantity { get; set; }
    public int? RawMaterialId { get; set; }
    public RawMaterial RawMaterial { get; set; }
}
