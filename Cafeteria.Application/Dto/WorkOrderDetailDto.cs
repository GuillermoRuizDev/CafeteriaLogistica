namespace Cafeteria.Application.Dto;

public record WorkOrderDetailDto(
    int? Id,
    int? WorkOrderId,
    double Quantity,
    decimal? Price,
    int ProductId
);