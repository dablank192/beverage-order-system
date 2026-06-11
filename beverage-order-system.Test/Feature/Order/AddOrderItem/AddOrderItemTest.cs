using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Feature.Order.AddOrderItem;
using beverage_order_system.Feature.Order.CreateOrder;
using beverage_order_system.Infrastructure;
using beverage_order_system.Model;
using Bogus;
using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace beverage_order_system.Test.Feature.Order.AddOrderDaily;

public class AddOrderItemTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    private readonly BeverageOrderFactory _factory;

    public AddOrderItemTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task AddOrderItemWithMockData()
    {
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


        //Create fake product
        var createFakeProduct = new Faker<Product>()
        .RuleFor(t => t.Name, f => f.Commerce.ProductName())
        .RuleFor(t => t.BasePrice, f => decimal.Parse(f.Commerce.Price(100000, 500000)))
        .RuleFor(t => t.CategoryId, f => new Random().Next(0, 9))
        .RuleFor(t => t.ProductImageUrl, f => f.Image.PicsumUrl())
        .RuleFor(t => t.IsAvailable, f => f.Random.Bool());

        var fakeProduct = createFakeProduct.Generate();

        db.Product.Add(fakeProduct);
        await db.SaveChangesAsync();

        // Generate fake order-item creation command for api
        var commandFaker = new Faker<SubCommand>()
        .CustomInstantiator(f => new SubCommand(
            ProductId: fakeProduct.Id
        ));

        var fakeOrderItem = commandFaker.Generate();


        // Create fake order
        var createFakeOrder = new beverage_order_system.Feature.Order.CreateOrder.Command();

        var newOrderResponse = await _client.PostAsJsonAsync("api/v1/order/new-order", createFakeOrder);

        Assert.Equal(HttpStatusCode.Created, newOrderResponse.StatusCode);

        var validOrderResponse = await newOrderResponse.Content.ReadFromJsonAsync<beverage_order_system.Feature.Order.CreateOrder.Result>();

        Assert.NotNull(validOrderResponse);


        // Fake api call to the addOrderItem Endpoint
        var response = await _client.PostAsJsonAsync($"api/v1/order/{validOrderResponse.OrderId}/new-order-item", fakeOrderItem);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
