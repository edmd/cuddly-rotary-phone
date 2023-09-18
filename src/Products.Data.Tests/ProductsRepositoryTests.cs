using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Products.Data.Persistence;
using Products.Data.Persistence.Entities;

namespace Products.Data.Tests
{
    //https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
    [TestFixture]
    public class ProductsRepositoryTests
    {
        private IProductsRepository _repository;
        private ProductsDbContext _ProductsDbContext;
        private Product _product;

        [SetUp]
        public void BeforeEach()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase("ProductsDb");
            _ProductsDbContext = new ProductsDbContext(optionsBuilder.Options);
            _repository = new ProductsRepository(_ProductsDbContext);

            _product = new Product(Guid.NewGuid(), int.MaxValue, "Widgets",
                "Widgets Description", 99M, Guid.NewGuid());
        }

        [Test]
        public async Task Should_save_successfully()
        {
            // Act
            var result = await _repository.Add(_product);

            // Assert
            result.Should().NotBeEmpty();
            Assert.IsInstanceOf(typeof(Guid), result);
        }

        [Test]
        public async Task Should_retrieve_successfully()
        {
            // Arrange
            await _repository.Add(_product);

            // Act
            var result = await _repository.Get(_product.Sku);

            // Assert
            Assert.Multiple(() =>
            {
                result?.Id.Should().Be(_product.Id);
                result?.Description.Should().Be(_product.Description);
                result?.Name.Should().Be(_product.Name);
                result?.Price.Should().Be(_product.Price);
                result?.SellerId.Should().Be(_product.SellerId);
                result?.Sku.Should().Be(_product.Sku);
            });
        }
    }
}