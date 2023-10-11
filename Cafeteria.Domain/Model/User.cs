namespace Cafeteria.Domain.Model;

public class User
{
    public User()
    {
        this.WorkOrders = new HashSet<WorkOrder>();
    }

    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Zone { get; set; }
    public ICollection<WorkOrder> WorkOrders { get; set; }
}
