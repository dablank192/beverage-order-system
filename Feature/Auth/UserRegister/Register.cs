using System;
using beverage_order_system.Exception.AuthException;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Auth.UserRegister;

public record Command (
    string Username,
    string Password
) : IRequest<Result>;
public record Result();

public class Register(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/register", async(
            ISender sender,
            Command req
        ) =>
        {
            await sender.Send(req);
            return Results.Created();
        })
        .WithName("User register")
        .Produces<Result>(StatusCodes.Status201Created)
        .ProducesValidationProblem(); 
    }

    public async Task<Result> Handle(Command req, CancellationToken ct)
    {
        var validUsername = await dbContext.User.FirstOrDefaultAsync(t => t.Username == req.Username, ct);

        if (validUsername != null) throw new DuplicateUsernameException();

        PasswordHasher<object> hash = new();

        var hashedPassword = hash.HashPassword(new object(), req.Password);

        var newUser = new User
        {
            Username= req.Username,
            Password= hashedPassword
        };

        dbContext.User.Add(newUser);
        await dbContext.SaveChangesAsync(ct);

        return new Result();
    }
}
