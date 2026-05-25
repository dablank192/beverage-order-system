using System;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using MediatR;

namespace beverage_order_system.Feature.Menu.AddMenuItem;

public class AddItem(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/new-item", async(
            ISender sender,
            Command req
        ) =>
        {
            await sender.Send(req);
            return Results.Created();
        })
        .WithName("Add Item to menu (Admin)")
        .RequireAuthorization()
        .Produces<Result>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var newItem = new Product
        {
            Name= req.Name,
            CategoryId= req.CategoryId,
            BasePrice= req.Price,
            ProductImageUrl= req.ProductImageUrl,
            IsAvailable= req.IsAvailable
        };

        dbContext.Product.Add(newItem);
        await dbContext.SaveChangesAsync(ct);

        return new Result();
    }
}
