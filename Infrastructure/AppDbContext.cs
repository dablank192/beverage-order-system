using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;

namespace beverage_order_system.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> option) : base(option) {}

    public DbSet<Categories> Categories {get; set;}
    public DbSet<Order> Order {get; set;}
    public DbSet<OrderItems> OrderItems {get; set;}
    public DbSet<OrderItemsTopping> OrderItemsTopping {get; set;}
    public DbSet<Product> Product {get; set;}
    public DbSet<Topping> Topping {get; set;}
    public DbSet<User> User {get; set;}
    public DbSet<RefreshToken> RefreshToken {get; set;}

    public void Configure (ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
