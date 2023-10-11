namespace Cafeteria.Domain.Model;

public class WorkOrder
{
    public WorkOrder()
    {
        this.WorkOrderDetails = new HashSet<WorkOrderDetail>();
    }
    public int WorkOrderId { get; set; }
    public int UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public string Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Taxe { get; set; }
    public decimal Total { get; set; }
    public User Users { get; set; }

    public ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }



}
