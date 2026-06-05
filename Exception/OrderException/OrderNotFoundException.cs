using System;

namespace beverage_order_system.Exception.OrderException;

public class OrderNotFoundException : System.Exception
{
    public OrderNotFoundException() : base(
        "Order Not Found"
    ) {}
}
