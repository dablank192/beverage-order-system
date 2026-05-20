using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class ToppingConfig : IEntityTypeConfiguration<Topping>
{
    public void Configure (EntityTypeBuilder<Topping> builder)
    {
        builder.ToTable("Topping");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
        .IsRequired()
        .HasMaxLength(50);
    }
}
