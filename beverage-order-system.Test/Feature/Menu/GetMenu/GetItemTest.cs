using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Dto;
using beverage_order_system.Feature.Menu.GetMenu;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using Bogus;
using Microsoft.Extensions.DependencyInjection;

namespace beverage_order_system.Test.Feature.Menu.GetMenu;

public class GetItemTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    private readonly BeverageOrderFactory _factory;

    public GetItemTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetItemTestAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Product.RemoveRange(db.Product);
        await db.SaveChangesAsync();

        var commandFaker = new Faker<Product>()
        .RuleFor(t => t.Name, f => f.Commerce.ProductName())
        .RuleFor(t => t.BasePrice, f => decimal.Parse(f.Commerce.Price(100000, 500000)))
        .RuleFor(t => t.CategoryId, f => new Random().Next(0, 9))
        .RuleFor(t => t.ProductImageUrl, f => f.Image.PicsumUrl())
        .RuleFor(t => t.IsAvailable, f => f.Random.Bool());

        var fakeProduct = commandFaker.Generate(20);

        db.Product.AddRange(fakeProduct);
        await db.SaveChangesAsync();    

        var response = await _client.GetAsync("api/v1/menu?PageIndex=1&PageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var validResult = await response.Content.ReadFromJsonAsync<Result>();

        Assert.NotNull(validResult);

        Assert.Equal(20, validResult.TotalRecord);
        Assert.Equal(1, validResult.CurrentPage);
        Assert.Equal(2, validResult.TotalPage);

        var dataList = validResult.Data as IEnumerable<MenuDto>;

        Assert.NotNull(dataList);
        Assert.Equal(10, dataList.Count());
    }
}
