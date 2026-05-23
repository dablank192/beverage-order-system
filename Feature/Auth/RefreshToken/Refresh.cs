using System;
using System.Net;
using beverage_order_system.Exception.AuthException;
using beverage_order_system.Exception.UserException;
using beverage_order_system.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Auth.RefreshToken;

public record Command(HttpContext Context) : IRequest<Result>;
public record Result();

public class Refresh(
    AppDbContext dbContext,
    IHelper helper
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/refresh", async(
            ISender sender,
            HttpContext httpContext
        ) =>
        {
            await sender.Send(new Command(httpContext));
            return Results.Ok();
        })
        .WithName("Refresh Token")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var context = req.Context;

        var rawRefreshToken = context.Request.Cookies["x-refresh-token"];

        // var decodedRefreshToken = WebUtility.UrlDecode(rawRefreshToken);

        var validatedToken = await dbContext.RefreshToken.FirstOrDefaultAsync(t => t.Token == rawRefreshToken
        && t.ExpiredAt > DateTime.UtcNow
        && t.IsRevoked == false, ct)
        ?? throw new InvalidRefreshTokenException();

        var user = await dbContext.User.FirstOrDefaultAsync(t => t.Id == validatedToken.UserId, ct);

        var newAccessToken = helper.GenerateJwtToken(user!);

        var newAccessCookies = new CookieOptions
        {
            HttpOnly= true,
            Secure= true,
            SameSite= SameSiteMode.Strict,
            Expires= DateTime.UtcNow.AddMinutes(15)
        };

        context.Response.Cookies.Append("x-access-token", newAccessToken, newAccessCookies);

        return new Result();
    }
}
