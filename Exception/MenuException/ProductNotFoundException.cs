using System;

namespace beverage_order_system.Exception.MenuException;

public class ProductNotFoundException : System.Exception
{
    public ProductNotFoundException() : base(
        "Product with the provided ID does not exist"
    ){}
}
