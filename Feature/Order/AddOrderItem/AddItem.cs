using System;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Order.AddOrderItem;

public record Command(
    int ProductId,
    Guid OrderId,
    int Quantity
) : IRequest<Result>;
public record Result();

public class AddItem(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/new-order-item", async(
            ISender sender,
            Command req
        ) =>
        {
            return Results.Ok(await sender.Send(req));
        })
        .WithName("Add Order Item")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle(Command req, CancellationToken ct)
    {
        var productPrice = await dbContext.Product.Where(t => t.Id == req.ProductId)
        .Select(t => t.BasePrice)
        .FirstOrDefaultAsync(ct);
        
        var newOrderItem = new OrderItems
        {
            ProductId = req.ProductId,
            OrderId = req.OrderId,
            Quantity = req.Quantity,
            UnitPrice = productPrice ?? default
        };

        dbContext.OrderItems.Add(newOrderItem);
        await dbContext.SaveChangesAsync(ct);

        return new Result();

        //tạm thời là vậy đã, chưa chốt logic, chưa test
    }
}
