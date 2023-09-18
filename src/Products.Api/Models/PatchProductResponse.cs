
namespace Products.Api.Models
{
    public class PatchProductResponse : Resource
    {
        public PatchProductResponse(int sku, string name, string description,
            decimal price, Guid sellerId)
        {
            Sku = sku;
            Name = name;
            Description = description;
            Price = price;
            SellerId = sellerId;
        }

        public int Sku { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public Guid? SellerId { get; set; }
    }
}
