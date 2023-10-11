using Cafeteria.Domain.Model;

namespace Cafeteria.Domain.ViewModel;

public class WorkOrderCreateViewModel
{
    public int UserId { get; set; }
    public DateTime CreationDate { get; set; }
    public string Status { get; set; }
    public Product Product { get; set; }
    public double Quantity { get; set; }
    public List<WorkOrderDetail> WorkOrderDetails { get; set; }
}
