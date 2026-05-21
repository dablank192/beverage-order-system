using System;
using beverage_order_system.DbConfig;
using beverage_order_system.Dto;
using beverage_order_system.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Feature.Menu.GetMenu;

public class GetItem(
    AppDbContext dbContext
) : IRequestHandler<Command, Result>

{
    public static void MapEndpoint (RouteGroupBuilder group)
    {
        group.MapGet("/", async(
            [FromServices] ISender sender,
            [AsParameters] Command req
        ) =>
        {
            return Results.Ok(await sender.Send(req));
        })
        .WithName("Get Menu")
        .AllowAnonymous()
        .Produces<Result>(StatusCodes.Status200OK);
    }

    public async Task<Result> Handle (Command req, CancellationToken ct)
    {
        var index = (req.PageIndex - 1) * req.PageSize;
        var totalRecord = await dbContext.Product.CountAsync(ct);
        
        var item = await dbContext.Product.
        AsNoTracking()
        .OrderByDescending(t => t.Id)
        .Skip(index)
        .Take(req.PageSize)
        .Select(t => new MenuDto(
            Id: t.Id,
            CategoryId: t.CategoryId,
            CategoryName: t.Categories!.Name,
            ProductName: t.Name,
            ProductPrice: t.BasePrice,
            ProductImageUrl: t.ProductImageUrl,
            IsAvailable: t.IsAvailable
        ))
        .ToListAsync(ct);

        var totalPage = totalRecord/req.PageSize;

        var result = new Result (
            Data: item,
            TotalRecord: totalRecord,
            CurrentPage: req.PageIndex,
            TotalPage: totalPage
        );

        return result;
    }
}
