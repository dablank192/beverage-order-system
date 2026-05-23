using System;
using beverage_order_system.Feature.Auth.RefreshToken;
using beverage_order_system.Feature.Auth.UserLogin;
using beverage_order_system.Feature.Auth.UserRegister;
using Carter;

namespace beverage_order_system.Feature.Auth;

public class AuthApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/auth")
        .WithTags("User Auth");

        Login.MapEndpoint(group);
        Refresh.MapEndpoint(group);
        Register.MapEndpoint(group);
    }
}
