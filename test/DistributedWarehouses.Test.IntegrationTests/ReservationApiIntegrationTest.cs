using System.Threading.Tasks;
using DistributedWarehouses.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace DistributedWarehouses.Test.IntegrationTests
{
    public class ReservationApiIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ReservationApiIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/v1/Invoices")]
        [InlineData("/v1/Items")]
        [InlineData("/v1/Reservations")]
        [InlineData("/v1/Warehouses")]
        public async Task GetHttpRequest(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}