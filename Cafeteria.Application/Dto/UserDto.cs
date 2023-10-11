using Cafeteria.Domain.Model;

namespace Cafeteria.Application.Dto;

public record UserDto(
    int Id,
    string Name,
    string Email,
    string Role,
    string IdentityId,
    ApplicationUser ApplicationUser,
    ICollection<WorkOrder> WorkOrders
);
