using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(
            DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .ToTable("Products");

            modelBuilder.Entity<Product>()
                .HasKey(x => x.id);

            modelBuilder.Entity<Product>()
                .Property(x => x.name)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(x => x.description)
                .HasColumnType("varchar(max)");

            modelBuilder.Entity<Product>()
                .Property(x => x.price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Product>()
                .Property(x => x.created_at)
                .HasColumnType("datetime2");

            modelBuilder.Entity<Product>()
                .Property(x => x.updated_at)
                .HasColumnType("datetime2");
        }
    }
}