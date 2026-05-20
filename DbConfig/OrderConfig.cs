using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure (EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.DailyOrderNumber)
        .IsUnique();
        builder.Property(t => t.DailyOrderNumber)
        .IsRequired();
    }
}
