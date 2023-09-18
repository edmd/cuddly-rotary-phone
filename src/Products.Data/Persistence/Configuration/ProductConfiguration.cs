using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product = Products.Data.Persistence.Entities.Product;

namespace ChambersCentral.Submissions.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.SellerId).IsRequired();
            builder.Property(x => x.Sku).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
