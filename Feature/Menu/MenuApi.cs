using System;
using beverage_order_system.Feature.Menu.AddMenuItem;
using beverage_order_system.Feature.Menu.GetMenu;
using Carter;

namespace beverage_order_system.Feature.GetMenu;

public class MenuApi : ICarterModule
{
    public void AddRoutes (IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/menu");

        GetItem.MapEndpoint(group); 
        AddItem.MapEndpoint(group);
    }
}
