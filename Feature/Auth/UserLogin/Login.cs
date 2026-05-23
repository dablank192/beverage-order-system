using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using beverage_order_system.Exception.UserException;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace beverage_order_system.Feature.Auth.UserLogin;

public record Command(
    string Username,
    string Password
) : IRequest<Result>;
public record Result();

public class Login (
    AppDbContext dbContext,
    HttpContext context,
    IHelper helper
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapPost("/login", async(
            ISender sender,
            Command req
        ) =>
        {
            await sender.Send(req);
            return Results.Ok();
        })
        .WithName("User login")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(t => t.Username == req.Username, ct)
        ?? throw new UserNotFoundException(req.Username);

        var accessToken = helper.GenerateJwtToken(user);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    
        var accessRefreshToken = new Model.RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiredAt = DateTime.Now.AddDays(3),
            IsRevoked = false
        };

        dbContext.RefreshToken.Add(accessRefreshToken);
        await dbContext.SaveChangesAsync(ct);

        var accessCookiesOption = new CookieOptions
        {
            HttpOnly= true,
            Secure= true,
            SameSite= SameSiteMode.Strict,
            Expires= DateTime.Now.AddMinutes(15)
        };

        context.Response.Cookies.Append("x-access-token", accessToken, accessCookiesOption);

        var refreshCookiesOption = new CookieOptions
        {
            HttpOnly= true,
            Secure= true,
            SameSite= SameSiteMode.Strict,
            Path= "/api/v1/auth/refresh",
            Expires= DateTime.Now.AddDays(3)
        };

        context.Response.Cookies.Append("x-refresh-token", refreshToken, refreshCookiesOption);

        return new Result();
    }
}
