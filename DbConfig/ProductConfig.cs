using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure (EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.CategoryId);

        builder.Property(t => t.Name)
        .IsRequired()
        .HasMaxLength(50);

        builder.HasOne(t => t.Categories)
        .WithMany(t => t.Products)
        .HasForeignKey(t => t.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
