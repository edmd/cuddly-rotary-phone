using System.ComponentModel.DataAnnotations;

namespace Products.Api.Models
{
    public class GetProductResponse : Resource
    {
        public GetProductResponse(int sku, string name, string description,
            decimal price, Guid sellerId)
        {
            Sku = sku;
            Name = name;
            Description = description;
            Price = price;
            SellerId = sellerId;
        }

        [Required]
        public int Sku { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public Guid? SellerId { get; set; }
    }
}