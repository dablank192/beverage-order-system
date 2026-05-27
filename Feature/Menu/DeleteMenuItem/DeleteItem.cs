using System;
using beverage_order_system.Exception.MenuException;
using beverage_order_system.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Menu.DeleteMenuItem;

public record Command(int ProductId) : IRequest<Result>;
public record Result(string Message);

public class DeleteItem(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{productId}/delete", async(
            ISender sender,
            int productId
        ) =>
        {
            return Results.Ok(await sender.Send(new Command(productId)));
        })
        .WithName("Delete Product")
        .Produces<Result>(StatusCodes.Status200OK)
        .RequireAuthorization();
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var product = await dbContext.Product.FirstOrDefaultAsync(t => t.Id == req.ProductId, ct)
        ?? throw new ProductNotFoundException();

        dbContext.Product.Remove(product);
        await dbContext.SaveChangesAsync(ct);

        return new Result("Item deleted successfully");
    }
}
