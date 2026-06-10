using System;
using beverage_order_system.Dto;
using beverage_order_system.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Order.GetOrderItem;

public record Query(
    Guid OrderId,
    SubQuery Params
) : IRequest<Result>;
public record SubQuery(
    int PageIndex = 1,
    int PageSize = 100
);
public record Result(
    List<OrderItemDto> Data,
    int TotalRecord,
    int CurrentPage,
    int TotalPage
);

public class GetOrder(
    AppDbContext dbContext
) : IRequestHandler<Query, Result>

{
    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{orderId}/get-item", async(
            [FromServices]ISender sender,
            [FromRoute]Guid orderId,
            [AsParameters]SubQuery req
        ) =>
        {
            return Results.Ok(await sender.Send(request: new Query(
                OrderId: orderId,
                Params: req
            )));
        })
        .WithName("Get Order Item")
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Query req, CancellationToken ct)
    {
        int index = (req.Params.PageIndex - 1) * req.Params.PageSize;

        int totalRecord = await dbContext.OrderItems.Where(t => t.OrderId == req.OrderId).CountAsync(ct);
        
        int totalPage = (int)Math.Ceiling(totalRecord/(double)req.Params.PageSize);

        var listOrder = await dbContext.OrderItems.Where(t => t.OrderId == req.OrderId)
        .AsNoTracking()
        .OrderByDescending(t => t.Quantity)
        .Skip(index)
        .Take(req.Params.PageSize)
        .Select(t => new OrderItemDto(
            ProductId: t.ProductId,
            OrderId: t.OrderId,
            Quantity: t.Quantity,
            UnitPrice: t.UnitPrice
        ))
        .ToListAsync(ct);

        var response = new Result(
            Data: listOrder,
            TotalRecord: totalRecord,
            CurrentPage: req.Params.PageIndex,
            TotalPage: totalPage
        );

        return response;

    }
}
