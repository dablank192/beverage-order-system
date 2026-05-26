using System;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Menu.UpdateMenuItem;

public class UpdateItem(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>
{
    public static void MapEndpoint(RouteGroupBuilder builder)
    {
        builder.MapPatch("/{productId}/item-update", async(
            [FromServices] ISender sender,
            int productId,
            [FromBody] SubCommand req
        ) =>
        {
            var result = new Command(productId, req);
            return Results.Ok(await sender.Send(result));
        })
        .WithName("Update Item")
        .Produces<Result>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequireAuthorization();
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var product = await dbContext.Product.FirstOrDefaultAsync(t => t.Id == req.ProductId, ct);

        var config = new TypeAdapterConfig();
        config.NewConfig<SubCommand, Product>().IgnoreNullValues(true);

        req.Data.Adapt(product, config);

        await dbContext.SaveChangesAsync(ct);

        return new Result();
    }
}
