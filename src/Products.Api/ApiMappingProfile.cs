using AutoMapper;
using Products.Api.Models;
using Products.Data.Persistence.Entities;

namespace Products.Api
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CreateProductRequest, Product>();

            CreateMap<PatchProductRequest, Product>();

            CreateMap<Product, GetProductResponse>();
        }
    }
}