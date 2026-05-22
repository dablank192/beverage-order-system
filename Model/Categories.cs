using System;

namespace beverage_order_system.Model;

public class Categories
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Description {get; set;}

    public List<Product>? Products {get; set;}
}
