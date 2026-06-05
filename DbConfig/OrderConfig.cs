using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace beverage_order_system.DbConfig;

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure (EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.DailyOrderNumber)
        .IsRequired();

        builder.Property(t => t.DailyOrderNumber)
        .IsRequired();
    }
}
