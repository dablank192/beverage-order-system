using System;
using beverage_order_system.Infrastructure;
using MediatR;

namespace beverage_order_system.Feature.Order.CreateOrder;

public record Command() : IRequest<Result>;
public record Result();

public class NewOrder(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapPost("/new-order", async(
            ISender sender,
            Command req
        ) =>
        {
            return Results.Ok(await sender.Send(req));
        })
        .WithName("Create New Order")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {   
        var newOrder = new Model.Order
        {
            DailyOrderNumber = new Random().Next(1, 100000),
            TotalAmount = 0,
            Status = Dto.OrderStatus.Pending,
            PayStatus = Dto.PaymentStatus.Unpaid
        };

        dbContext.Order.Add(newOrder);
        await dbContext.SaveChangesAsync(ct);

        return new Result();
    }
}
