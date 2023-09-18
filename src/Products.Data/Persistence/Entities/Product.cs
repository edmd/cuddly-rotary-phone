
namespace Products.Data.Persistence.Entities
{
    public class Product : BaseEntity
    {
        public Product() { }

        public Product(Guid id, int sku, string name, string description,
            decimal price, Guid sellerId)
        {
            Id = id;
            Sku = sku;
            Name = name;
            Description = description;
            Price = price;
            SellerId = sellerId;
        }

        public Guid Id { get; set; }

        public int Sku { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public Guid? SellerId { get; set; }
    }
}