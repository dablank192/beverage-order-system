using System;
using beverage_order_system.Feature.Order.AddOrderItem;
using beverage_order_system.Feature.Order.CreateOrder;
using beverage_order_system.Feature.Order.GetOrderItem;
using Carter;

namespace beverage_order_system.Feature.Order;

public class OrderApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/order")
        .WithTags("Order Item Management");

        AddItem.MapEndpoint(group);
        NewOrder.MapEndpoint(group);
        GetOrder.MapEndpoint(group);
    }
}
