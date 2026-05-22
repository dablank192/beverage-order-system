using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure (EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.UserId);
        builder.Property(t => t.UserId)
        .IsRequired();

        builder.HasOne(t => t.User)
        .WithMany(t => t.RefreshToken)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.Token)
        .IsUnique();
    }
}
