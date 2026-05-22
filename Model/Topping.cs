using System;

namespace beverage_order_system.Model;

public class Topping
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public decimal? Price {get; set;}
    public bool? IsAvailable {get; set;}

    public List<OrderItemsTopping>? OrderItemsToppings {get; set;}
}
