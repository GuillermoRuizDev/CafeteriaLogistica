using Cafeteria.Domain.Model;

namespace Cafeteria.Application.Dto;

public record ProductRawMaterialDto(
    int ProductRawMaterialId,
    int ProductId,
    int RawMaterialId,
    int Quantity,
    Product Product,
    RawMaterial RawMaterial
);
