using System;

namespace beverage_order_system.Dto;

public record OrderItemDto
(
    int? ProductId,
    Guid? OrderId,
    int? Quantity,
    decimal? UnitPrice
);