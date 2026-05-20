using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class OrderItemsConfig : IEntityTypeConfiguration<OrderItems>
{
    public void Configure (EntityTypeBuilder<OrderItems> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.ProductId);

        builder.HasIndex(t => t.OrderId);

        builder.HasOne(t => t.Order)
        .WithMany(t => t.OrderItems)
        .HasForeignKey(t => t.OrderId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Product)
        .WithMany(t => t.OrderItems)
        .HasForeignKey(t => t.ProductId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
