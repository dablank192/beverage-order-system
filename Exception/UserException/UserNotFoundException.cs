using System;

namespace beverage_order_system.Exception.UserException;

public class UserNotFoundException : System.Exception
{
    public UserNotFoundException(string userName) : base(
        $"User not found: Invalid Username [{userName}]"
    ){}
}