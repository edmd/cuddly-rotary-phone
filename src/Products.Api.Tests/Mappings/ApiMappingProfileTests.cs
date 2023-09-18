using AutoMapper;
using FluentAssertions;
using Products.Api.Models;
using Products.Data.Persistence.Entities;

namespace Products.Api.Tests.Mappings
{
    [TestFixture]
    public class ApiMappingProfileTests
    {
        private readonly IMapper _mapper;
        public ApiMappingProfileTests()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<ApiMappingProfile>(); })
                .CreateMapper();
        }

        [Test]
        public void Should_map_CreateProductRequest_properties()
        {
            //CreateMap<CreateProductRequest, Product>();

            //Arrange
            var source = new CreateProductRequest(
                sku: int.MaxValue, 
                name: "Widgets", 
                description: "Widgets Description", 
                price: 99M, 
                sellerId: Guid.NewGuid()
            );

            //Act
            var result = _mapper.Map<Product>(source);

            //Assert
            Assert.Multiple(() =>
            {
                result.Sku.Should().Be(source.Sku);
                result.SellerId.Should().Be(source.SellerId);
                result.Description.Should().Be(source.Description);
                result.Name.Should().Be(source.Name);
                result.Price.Should().Be(source.Price);
            });
        }

        [Test]
        public void Should_map_GetProductRequest_properties()
        {
            //CreateMap<Product, GetProductResponse>()

            //Arrange
            var source = new Product(
                id: Guid.NewGuid(), 
                description: Guid.NewGuid().ToString(), 
                name: Guid.NewGuid().ToString(),
                price: 99M,
                sellerId: Guid.NewGuid(),
                sku: int.MaxValue
            );

            //Act
            var result = _mapper.Map<GetProductResponse>(source);

            //Assert
            Assert.Multiple(() =>
            {
                //result.Id.Should().Be(source.Id);
                result.Description.Should().Be(source.Description);
                result.Name.Should().Be(source.Name);
                result.Price.Should().Be(source.Price);
                result.SellerId.Should().Be(source.SellerId);
                result.Sku.Should().Be(source.Sku);
            });
        }
    }
}