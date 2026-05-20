using System;

namespace beverage_order_system.Model;

public class OrderItemsTopping
{
    public int Id {get; set;}
    public int OrderItemsId {get; set;}
    public int ToppingId {get; set;}
    public decimal ToppingPrice {get; set;}

    public Topping? Topping {get; set;}
    public OrderItems? OrderItems {get; set;}
}
