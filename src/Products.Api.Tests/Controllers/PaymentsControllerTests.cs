using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Products.Api.Controllers;
using Products.Api.Models;

namespace Products.Api.Tests.Controllers
{
    [TestFixture]
    public class productsControllerTests
    {
        private readonly ProductsController _sut;
        private readonly IMapper _mapper;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IHttpContextAccessor> _accessorMock;

        public productsControllerTests()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<ApiMappingProfile>(); })
                .CreateMapper();

            _mediatorMock = new Mock<IMediator>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateProductResponse(int.MaxValue))
                .Verifiable("CreateProductRequest was not sent.");

            _accessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = HostString.FromUriComponent("https:localhost");
            context.Request.Path = PathString.Empty;
            _accessorMock.Setup(_ => _.HttpContext).Returns(context);

            _sut = new ProductsController(NullLogger<ProductsController>.Instance, _mapper, _mediatorMock.Object, _accessorMock.Object) { };
        }

        [Test]
        public async Task Should_create_product_successfully()
        {
            // Arrange
            var product = new CreateProductRequest(
                123, "Widgets", "Widgets Description", 99.99M, Guid.NewGuid());

            //Act
            IActionResult result = await _sut.Create(product);

            //Assert
            _mediatorMock.Verify(x => x.Send(
                It.IsAny<CreateProductRequest>(), 
                It.IsAny<CancellationToken>()), Times.Once());

            Assert.Multiple(() =>
            {
                result.Should().BeAssignableTo<CreatedResult>();
                var createdResult = result as CreatedResult;
                createdResult?.Value.Should().BeAssignableTo<CreateProductResponse>();
                var value = createdResult?.Value as CreateProductResponse;
                value?.Sku.Should().HaveValue();
            });
        }

        //public async Task Should_create_product_unsuccessfully() { }
        //public async Task Should_get_product_successfully() { }
        //public async Task Should_get_product_unsuccessfully() { }
    }
}