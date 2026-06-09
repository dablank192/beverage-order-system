using System;

namespace beverage_order_system.Exception.OrderException;

public class DeleteOrderJobException : System.Exception
{
    public DeleteOrderJobException() : base(
        "Can not execute cronjob 'DeleteOrderException'"
    ){}
}
