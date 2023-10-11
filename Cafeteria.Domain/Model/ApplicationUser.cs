using Microsoft.AspNetCore.Identity;

namespace Cafeteria.Domain.Model;

public class ApplicationUser : IdentityUser
{
    public int? UserId { get; set; }
    public User User { get; set; }
}
