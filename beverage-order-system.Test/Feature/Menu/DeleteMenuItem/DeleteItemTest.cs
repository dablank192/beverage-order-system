using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Feature.Menu.DeleteMenuItem;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace beverage_order_system.Test.Feature.Menu.DeleteMenuItem;

public class DeleteItemTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    private readonly BeverageOrderFactory _factory;

    public DeleteItemTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestAuth");
    }

    [Fact]
    public async Task DeleteItemMockData()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var fakeProduct = new Faker<Product>()
        .RuleFor(t => t.Name, f => f.Commerce.ProductName())
        .RuleFor(t => t.CategoryId, f => f.Random.Int(0, 9))
        .RuleFor(t => t.BasePrice, f => decimal.Parse(f.Commerce.Price()))
        .RuleFor(t => t.ProductImageUrl, f => f.Image.PicsumUrl())
        .RuleFor(t => t.IsAvailable, f => f.Random.Bool());

        var fakeRecord = fakeProduct.Generate(3);
        db.Product.AddRange(fakeRecord);
        await db.SaveChangesAsync();

        var fakeProductId = fakeRecord[0].Id;
        db.ChangeTracker.Clear();

        var response = await _client.DeleteAsync($"api/v1/menu/{fakeProductId}/delete");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        var readResponse = await response.Content.ReadFromJsonAsync<Result>();

        Assert.Equal("Item deleted successfully", readResponse!.Message);


        var productInFakeDb = await db.Product.FirstOrDefaultAsync(t => t.Id == fakeProductId);

        Assert.Null(productInFakeDb); 
    }
}
