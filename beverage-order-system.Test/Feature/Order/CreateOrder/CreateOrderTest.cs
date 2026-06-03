using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Feature.Order.CreateOrder;
using beverage_order_system.Infrastructure;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace beverage_order_system.Test.Feature.Order.CreateOrder;

public class CreateOrderTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    private readonly BeverageOrderFactory _factory;

    public CreateOrderTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task NewOrderWithMockData()
    {
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Order.RemoveRange();
        await db.SaveChangesAsync();

        var emptyCommand = new Command();

        var response = await _client.PostAsJsonAsync("api/v1/order/new-order", emptyCommand);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var validResponse = await response.Content.ReadFromJsonAsync<Result>();
        
        var addedFakeOrder = await db.Order.FirstOrDefaultAsync(t => t.Id == validResponse.OrderId);

        Assert.NotNull(addedFakeOrder);
    }
}
