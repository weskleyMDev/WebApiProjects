using CatalogoAPIMinimal.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoAPIMinimal.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Category>().HasKey(c => c.CategoryId);
        mb.Entity<Category>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        mb.Entity<Category>().Property(c => c.Description).HasMaxLength(150).IsRequired();

        mb.Entity<Product>().HasKey(p => p.ProductId);
        mb.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        mb.Entity<Product>().Property(p => p.Description).HasMaxLength(150).IsRequired();
        mb.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2);

        mb.Entity<Product>().HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId);
    }
}