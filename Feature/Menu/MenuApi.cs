using System;
using beverage_order_system.Feature.Menu.AddMenuItem;
using beverage_order_system.Feature.Menu.DeleteMenuItem;
using beverage_order_system.Feature.Menu.GetMenu;
using beverage_order_system.Feature.Menu.UpdateMenuItem;
using Carter;

namespace beverage_order_system.Feature.GetMenu;

public class MenuApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/menu");

        GetItem.MapEndpoint(group); 
        AddItem.MapEndpoint(group);
        UpdateItem.MapEndpoint(group);
        DeleteItem.MapEndpoint(group);
    }
}
