using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Feature.Order.CreateOrder;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace beverage_order_system.Test.Feature.Order.GetOrder;

public class GetOrderTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    private readonly BeverageOrderFactory _factory;

    public GetOrderTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetOrderWithMockData()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.OrderItems.RemoveRange();
        await db.SaveChangesAsync();

        var fakeOrderCommand = new Command();

        var createOrderResponse = await _client.PostAsJsonAsync("api/v1/order/new-order", fakeOrderCommand);

        var validOrderResponse = await createOrderResponse.Content.ReadFromJsonAsync<Result>();

        var createFakeProduct = new Faker<Product>()
        .RuleFor(t => t.Name, f => f.Commerce.ProductName())
        .RuleFor(t => t.BasePrice, f => decimal.Parse(f.Commerce.Price(100000, 500000)))
        .RuleFor(t => t.CategoryId, f => new Random().Next(0, 9))
        .RuleFor(t => t.ProductImageUrl, f => f.Image.PicsumUrl())
        .RuleFor(t => t.IsAvailable, f => f.Random.Bool());

        var fakeProduct = createFakeProduct.Generate();
        db.Product.Add(fakeProduct);
        await db.SaveChangesAsync();

        
        var fakeOrderItemCommand = new Faker<OrderItems>()
        .RuleFor(t => t.ProductId, f => fakeProduct.Id)
        .RuleFor(t => t.OrderId, f => validOrderResponse.OrderId)
        .RuleFor(t => t.Quantity, f => f.Random.Int(1, 9))
        .RuleFor(t => t.UnitPrice, f => decimal.Parse(f.Commerce.Price(100000, 900000)));

        var fakeOrderItem = fakeOrderItemCommand.Generate();

        db.OrderItems.Add(fakeOrderItem);
        await db.SaveChangesAsync();

        var response = await _client.GetAsync($"api/v1/order/{validOrderResponse.OrderId}/get-item?pageIndex=1&pageSize=100");

        var rawResponse = await response.Content.ReadAsStringAsync();

        Assert.True(response.IsSuccessStatusCode, $"API Failed! Status: {response.StatusCode}. Body: {rawResponse}");

        var validOrderItemResponse = await response.Content.ReadFromJsonAsync<beverage_order_system.Feature.Order.GetOrderItem.Result>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.NotNull(validOrderItemResponse);

        Assert.Equal(1, validOrderItemResponse.TotalRecord);
        Assert.Equal(1, validOrderItemResponse.CurrentPage);
        Assert.Equal(1, validOrderItemResponse.TotalPage);
    }
}
