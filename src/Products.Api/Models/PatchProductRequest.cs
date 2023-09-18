using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Products.Api.Models
{
    public class PatchProductRequest : IRequest<PatchProductResponse>
    {
        public PatchProductRequest(string name, string description, 
            decimal price, Guid sellerId)
        {
            Name = name;
            Description = description;
            Price = price;
            SellerId = sellerId;
        }

        [MaxLength(50)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public Guid? SellerId { get; set; }
    }
}
