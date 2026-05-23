using System;

namespace beverage_order_system.Exception.AuthException;

public class InvalidCredentialException : System.Exception
{
    public InvalidCredentialException() : base(
        "Invalid Username or Password"
    ){}
}
