using EComMicroServApi.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EComMicroServApi.Api.Data.Config;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(80);

        builder.Property(p => p.Description).IsRequired().HasMaxLength(120);

        builder.Property(p => p.Price).HasPrecision(7, 2);

        builder.Property(p => p.ImageUrl).IsRequired().HasMaxLength(500);

        builder.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
    }
}