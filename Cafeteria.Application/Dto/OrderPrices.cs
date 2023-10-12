namespace Cafeteria.Application.Dto;

public record OrderPrices(decimal Subtotal,
            decimal Impuesto,
            decimal Total);