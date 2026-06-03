using System;
using beverage_order_system.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace beverage_order_system.Feature.Order.CreateOrder;

public record Command() : IRequest<Result>;
public record Result(
    Guid OrderId
);

public class NewOrder(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapPost("/new-order", async(
            [FromServices]ISender sender,
            [FromBody]Command req
        ) =>
        {
            var result = await sender.Send(req);
            return Results.Created(string.Empty, result);
        })
        .WithName("Create New Order")
        .Produces<Result>(StatusCodes.Status201Created);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {   
        var newOrder = new Model.Order
        {
            TotalAmount = 0,
            Status = Dto.OrderStatus.Pending,
            PayStatus = Dto.PaymentStatus.Unpaid
        };

        dbContext.Order.Add(newOrder);
        await dbContext.SaveChangesAsync(ct);

        return new Result(OrderId: newOrder.Id);
    }
}
