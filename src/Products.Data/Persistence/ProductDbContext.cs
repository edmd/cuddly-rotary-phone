using Microsoft.EntityFrameworkCore;
using Products.Data.Persistence.Entities;

namespace Products.Data.Persistence
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext() { }
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }
        protected ProductsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder != null && !optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("ProductsDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}