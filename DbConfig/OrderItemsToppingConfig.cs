using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class OrderItemsToppingConfig : IEntityTypeConfiguration<OrderItemsTopping>
{
    public void Configure (EntityTypeBuilder<OrderItemsTopping> builder)
    {
        builder.ToTable("OrderItemsTopping");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.OrderItemsId);
        
        builder.HasIndex(t => t.ToppingId);

        builder.HasOne(t => t.OrderItems)
        .WithMany(t => t.OrderItemsToppings)
        .HasForeignKey(t => t.OrderItemsId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Topping)
        .WithMany(t => t.OrderItemsToppings)
        .HasForeignKey(t => t.ToppingId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
