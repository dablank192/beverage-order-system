using System;

namespace beverage_order_system.Model;

public class OrderItems
{
    public int Id {get; set;}
    public int ProductId {get; set;}
    public Guid OrderId {get; set;}
    public int Quantity {get; set;}
    public decimal UnitPrice {get; set;}

    public Order? Order {get; set;}
    public Product? Product {get; set;}
    public List<OrderItemsTopping>? OrderItemsToppings {get; set;}
}
