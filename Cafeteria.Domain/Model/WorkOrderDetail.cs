namespace Cafeteria.Domain.Model;

public class WorkOrderDetail
{
    public int WorkOrderDetailId { get; set; }
    public int WorkOrderId { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public virtual WorkOrder WorkOrder { get; set; }
}
