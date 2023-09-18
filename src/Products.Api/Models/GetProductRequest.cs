using MediatR;

namespace Products.Api.Models
{
    public class GetProductRequest : IRequest<GetProductResponse>
    {
        public GetProductRequest(int sku, string name)
        {
            Sku = sku;
            Name = name;
        }

        public int Sku { get; set; }

        public string Name { get; set; }
    }
}