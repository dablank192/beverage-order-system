using System;

namespace beverage_order_system.Exception.AuthException;

public class InvalidRefreshTokenException : System.Exception
{
    public InvalidRefreshTokenException() : base(
        "Invalid Refresh Token: Can't identify refresh token"
    ){}
}
