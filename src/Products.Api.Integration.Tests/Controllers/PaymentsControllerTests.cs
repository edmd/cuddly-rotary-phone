using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Products.Api.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Products.Api.Integration.Tests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private TestWebApplicationFactory _factory;
        private HttpClient _client;

        public ProductsControllerTests()
        {
            _factory = new TestWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Should_create_successfully()
        {
            // Arrange
            var product = new CreateProductRequest(123, "Widgets", "Widgets Description", 99.99M, Guid.NewGuid());

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.ProductsApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/products", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
            var responseModel = JsonConvert.DeserializeObject<CreateProductResponse>(
                response.Content.ReadAsStringAsync().Result);

            Assert.Multiple(() =>
            {
                responseModel.Should().NotBeNull();
                responseModel.Should().BeOfType<CreateProductResponse>();
                responseModel?.Sku.Should().HaveValue();
            });
        }

        [Test]
        public async Task Should_create_bad_request()
        {
            // Arrange
            var product = new CreateProductRequest(123, "Widgets", 
                "Widgets Description12345678901234567890123456789012345678901234567890", 99.99M, Guid.NewGuid());

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.ProductsApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/products", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.Multiple(() =>
            {
                responseContent.Should().NotBeNull();
                responseContent.Should().Contain("The CardHolderName field is required");
                responseContent.Should().Contain("400");
            });
        }

        [Test]
        public async Task Should_create_unauthorised()
        {
            // Arrange
            var product = new CreateProductRequest(123, "Widgets", "Widgets Description", 99.99M, Guid.NewGuid());

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/products", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Should_get_successfully() {
            // Arrange
            var product = new CreateProductRequest(123, "Widgets", "Widgets Description", 99.99M, Guid.NewGuid());

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.ProductsApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/products", UriKind.Relative), stringContent);
            var responseModel = JsonConvert.DeserializeObject<CreateProductResponse>(
                response.Content.ReadAsStringAsync().Result);

            var getResponse = await _client.GetAsync($"https://localhost:7005/api/products/{responseModel?.Sku}");
            var content = await getResponse.Content.ReadAsStringAsync();
            var productResponse = JsonConvert.DeserializeObject<GetProductResponse>(content);

            // Assert
            Assert.Multiple(() =>
            {
                productResponse.Should().NotBeNull();
                productResponse.Should().BeOfType<GetProductResponse>();
                productResponse?.Sku.Should().Be(product.Sku);
                productResponse?.Description.Should().Be(product.Description);
                productResponse?.Name.Should().Be(product.Name);
                productResponse?.Price.Should().Be(product.Price);
                productResponse?.SellerId.Should().Be(product.SellerId);
            });
        }

        [Test]
        public async Task Should_get_not_found() {
            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.ProductsApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getResponse = await _client.GetAsync($"https://localhost:7005/api/products/{Guid.NewGuid()}");
            var content = await getResponse.Content.ReadAsStringAsync();
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(content);

            // Assert
            Assert.Multiple(() =>
            {
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
                errorDetails?.Should().NotBeNull();
                errorDetails?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
                errorDetails?.Message.Should().Be("Product not found in datastore");
            });
        }

        [Test]
        public async Task Should_get_unauthorised() {
            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getResponse = await _client.GetAsync($"https://localhost:7005/api/products/{Guid.NewGuid()}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}