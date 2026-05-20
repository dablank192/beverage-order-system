using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class CategoriesConfig : IEntityTypeConfiguration<Categories>
{
    public void Configure (EntityTypeBuilder<Categories> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
        .IsRequired()
        .HasMaxLength(50);
        builder.HasIndex(t => t.Name);

        builder.Property(t => t.Description)
        .IsRequired(false)
        .HasMaxLength(200);
    }
}
