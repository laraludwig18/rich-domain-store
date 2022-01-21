using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RichDomainStore.API.IntegrationTests.Fixtures;
using RichDomainStore.API.Models;
using Xunit;

namespace RichDomainStore.API.IntegrationTests
{
    [Collection(nameof(IntegrationTestsCollection))]
    public class CartControllerIntegrationTests
    {
        private readonly IntegrationTestsFixture _testsFixture;
        private readonly AddOrderItemRequest _addOrderItemRequest;
        public CartControllerIntegrationTests(IntegrationTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _addOrderItemRequest = new AddOrderItemRequest
            {
                ProductId = _testsFixture.DatabaseFixture.Products.FirstOrDefault().Id,
                Quantity = 2
            };
        }

        [Fact]
        public async Task AddItem_NewOrder_ShouldReturnSuccessfully()
        {
            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/v1/carts/items", _addOrderItemRequest);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task RemoveItem_ExistingOrder_ShouldReturnSuccessfully()
        {
            // Arrange
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/v1/carts/items", _addOrderItemRequest);
            postResponse.EnsureSuccessStatusCode();

            // Act
            var deleteResponse = await _testsFixture.Client.DeleteAsync(
                $"api/v1/carts/items/{_addOrderItemRequest.ProductId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}