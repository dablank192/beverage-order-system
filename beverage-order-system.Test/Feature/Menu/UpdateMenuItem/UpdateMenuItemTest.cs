using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Feature.Menu.UpdateMenuItem;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace beverage_order_system.Test.Feature.Menu.UpdateMenuItem;

public class UpdateMenuItemTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    private readonly BeverageOrderFactory _factory;

    public UpdateMenuItemTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestAuth");
    }

    [Fact]
    public async Task UpdateItemWithMockData()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Product.RemoveRange(db.Product);
        await db.SaveChangesAsync();

        var fakeProduct = new Faker<Product>()
        .RuleFor(t => t.Name, f => f.Commerce.ProductName())
        .RuleFor(t => t.CategoryId, f => new Random().Next(0, 9))
        .RuleFor(t => t.BasePrice, f => decimal.Parse(f.Commerce.Price(0, 1000000)))
        .RuleFor(t => t.ProductImageUrl, f => f.Image.PicsumUrl())
        .RuleFor(t => t.IsAvailable, true);

        var fakeRecord = fakeProduct.Generate(3);

        db.Product.AddRange(fakeRecord);
        await db.SaveChangesAsync();

        var fakeProductId = fakeRecord[0].Id;

        var commandFaker = new Faker<SubCommand>()
        .CustomInstantiator(f => new SubCommand(
            Name: "success",
            CategoryId: new Random().Next(0, 9),
            BasePrice: decimal.Parse(f.Commerce.Price(0, 1000000)),
            IsAvailable: false
        ));

        var payload = commandFaker.Generate();

        var response = await _client.PatchAsJsonAsync($"api/v1/menu/{fakeProductId}/item-update", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        db.ChangeTracker.Clear();

        var updatedFakeProduct = await db.Product.FirstOrDefaultAsync(t => t.Id == fakeProductId);

        Assert.NotNull(updatedFakeProduct);
        Assert.Equal("success", updatedFakeProduct.Name);
        Assert.Equal(payload.BasePrice, updatedFakeProduct.BasePrice);
        Assert.False(updatedFakeProduct.IsAvailable);
    }
}
