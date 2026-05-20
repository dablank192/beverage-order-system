using System;

namespace beverage_order_system.Model;

public class Product
{
    public int Id {get; set;}
    public int CategoryId {get; set;}
    public required string Name {get; set;}
    public decimal? BasePrice {get; set;}
    public string? ProductImageUrl {get; set;}
    public bool? IsAvailable {get; set;}

    public List<OrderItems>? OrderItems {get; set;}
    public Categories? Categories {get; set;}
}
