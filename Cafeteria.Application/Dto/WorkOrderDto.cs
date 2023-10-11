using Cafeteria.Domain.Model;

namespace Cafeteria.Application.Dto;

public record WorkOrderDto(
    int? Id,
    DateTime? CreationDate,
    string? Status,
    ICollection<Product> Products
);