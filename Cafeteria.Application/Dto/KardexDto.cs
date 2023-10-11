namespace Cafeteria.Application.Dto;

public record KardexDto(
     int KardexId,
     DateTime TransactionDate,
     string Description,
     decimal Quantity,
     string NameProduct
);

