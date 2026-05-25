using System;
using System.Net;
using System.Net.Http.Json;
using beverage_order_system.Feature.Menu.AddMenuItem;
using Bogus;

namespace beverage_order_system.Test.Feature.Menu.AddMenuItem;

public class AddMenuItemTest : IClassFixture<BeverageOrderFactory>
{
    private readonly HttpClient _client;
    
    public AddMenuItemTest(BeverageOrderFactory factory)
    {
        _client = factory.CreateClient();

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("TestAuth");
    }

    [Fact]
    public async Task AddItemWithMockData()
    {
        var commandFaker = new Faker<Command>()
        .CustomInstantiator(f => new Command(
            Name: f.Commerce.ProductName(),
            CategoryId: 0,
            Price: decimal.Parse(f.Commerce.Price(100000, 500000)),
            ProductImageUrl: f.Image.PicsumUrl(),
            IsAvailable: f.Random.Bool()
        ));

        var fakeCommand = commandFaker.Generate();

        var response = await _client.PostAsJsonAsync("api/v1/menu/new-item", fakeCommand);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
