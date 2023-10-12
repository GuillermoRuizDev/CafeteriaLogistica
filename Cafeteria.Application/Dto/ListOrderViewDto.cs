namespace Cafeteria.Application.Dto;

public record ListOrderViewDto(
        string User,
        string Product,
        string CreationDate,
        string Status,
        double Quantity
    );
