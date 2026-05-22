using System;
using beverage_order_system.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace beverage_order_system.DbConfig;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure (EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Username)
        .HasMaxLength(50);
        builder.HasIndex(t => t.Username);

        builder.Property(t => t.Email)
        .HasMaxLength(20);
    }
}
