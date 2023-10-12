namespace Cafeteria.Application.Dto;

public record KardexDto(
     int KardexId,
     string TransactionDate,
     string Description,
     decimal Quantity,
     string NameProduct
);

