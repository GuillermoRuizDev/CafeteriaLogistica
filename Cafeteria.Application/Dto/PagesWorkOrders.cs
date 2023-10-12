using Cafeteria.Domain.Model;

namespace Cafeteria.Application.Dto;

public record PagesWorkOrders(int TotalItems,
        int TotalPages,
        int Page,
        int PageSize,
        List<WorkOrder> WorkOrders);
