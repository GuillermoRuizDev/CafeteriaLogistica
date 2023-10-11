using Cafeteria.Domain.Model;

namespace Cafeteria.Application.Dto;

public record RawMaterialDto(
    int RawMaterialId,
    string Name,
    int AvailableQuantity,
    ICollection<ProductRawMaterial> ProductRawMaterials
);
