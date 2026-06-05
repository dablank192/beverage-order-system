using System;
using beverage_order_system.Exception.OrderException;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Order.AddOrderItem;

public record Command(
    Guid OrderId,
    SubCommand Data
) : IRequest<Result>;
public record SubCommand(
    int ProductId
);
public record Result();

public class AddItem(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/{orderId}/new-order-item", async(
            [FromServices]ISender sender,
            [FromRoute]Guid orderId,
            [FromBody]SubCommand req
        ) =>
        {
            return Results.Ok(await sender.Send(new Command(orderId, req)));
        })
        .WithName("Add Order Item")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle(Command req, CancellationToken ct)
    {
        var item = await dbContext.Order
        .Where(t => t.Id == req.OrderId)
        .FirstOrDefaultAsync(ct)
        ?? throw new OrderNotFoundException();

        var productPrice = await dbContext.Product.Where(t => t.Id == req.Data.ProductId)
        .Select(t => t.BasePrice)
        .FirstOrDefaultAsync(ct);

        var existingItem = await dbContext.OrderItems.FirstOrDefaultAsync(t => t.OrderId == req.OrderId 
        && t.ProductId == req.Data.ProductId, ct);

        if (existingItem != null)
        {
            existingItem.Quantity ++;
        }
        else
        {    
            var newOrderItem = new OrderItems
            {
                ProductId = req.Data.ProductId,
                OrderId = req.OrderId,
                Quantity = 1,
                UnitPrice = productPrice ?? default
            };
            dbContext.OrderItems.Add(newOrderItem);
        }

        item.TotalAmount = (item.TotalAmount ?? 0) + productPrice;
        
        await dbContext.SaveChangesAsync(ct);

        return new Result();

    }
}
