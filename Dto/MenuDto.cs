using System;

namespace beverage_order_system.Dto;

public record MenuDto
(
    int Id,
    int CategoryId,
    string CategoryName,
    string ProductName,
    decimal? ProductPrice,
    string? ProductImageUrl,
    bool? IsAvailable
);
