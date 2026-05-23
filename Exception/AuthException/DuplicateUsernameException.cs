using System;

namespace beverage_order_system.Exception.AuthException;

public class DuplicateUsernameException : System.Exception
{
    public DuplicateUsernameException() : base(
        "User already existed"
    ){}
}
