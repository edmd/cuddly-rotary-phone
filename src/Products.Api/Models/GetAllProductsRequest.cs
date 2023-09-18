using MediatR;

namespace Products.Api.Models
{
    public class GetAllProductsRequest : IRequest<List<GetProductResponse>>
    {
    }
}