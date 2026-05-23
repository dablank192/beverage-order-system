using System;
using beverage_order_system.Exception.AuthException;
using beverage_order_system.Exception.UserException;
using beverage_order_system.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Auth.RefreshToken;

public record Command() : IRequest<Result>;
public record Result();

public class Refresh(
    AppDbContext dbContext,
    HttpContext context,
    IHelper helper
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/refresh", async(
            ISender sender,
            Command req
        ) =>
        {
            await sender.Send(req);
            return Results.Ok();
        })
        .WithName("Refresh Token")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var refreshToken = context.Request.Cookies["x-refresh-token"];

        var validatedToken = await dbContext.RefreshToken.FirstOrDefaultAsync(t => t.Token == refreshToken
        && t.ExpiredAt > DateTime.Now
        && t.IsRevoked == false, ct)
        ?? throw new InvalidRefreshTokenException();

        var user = await dbContext.User.FirstOrDefaultAsync(t => t.Id == validatedToken.User!.Id, ct);

        var newAccessToken = helper.GenerateJwtToken(user!);

        var newAccessCookies = new CookieOptions
        {
            HttpOnly= true,
            Secure= true,
            SameSite= SameSiteMode.Strict,
            Expires= DateTime.Now.AddMinutes(15)
        };

        context.Response.Cookies.Append("x-access-key", newAccessToken, newAccessCookies);

        return new Result();
    }
}
