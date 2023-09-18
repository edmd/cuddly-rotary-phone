
namespace Products.Api.Models
{
    public class CreateProductResponse
    {
        public CreateProductResponse() { }

        public CreateProductResponse(int sku)
        {
            Sku = sku;
        }

        public int? Sku { get; set; }
    }
}