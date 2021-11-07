using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DistributedWarehouses.Test.IntegrationTests.Controllers
{
    public class ControllersTest
    {
        private readonly HttpClient _httpClient;
        private const string BaseAddress = "https://localhost:5001/v1/";

        public ControllersTest()
        {
            _httpClient = new HttpClient();
        }
        [Fact]
        public async Task CorrectNewReservationInput_Reserve_ReturnsNewReservationId()
        {
            var body = "{\r\n  \"itemSku\": \"string\",\r\n  \"quantity\": 0\r\n}";
            var response = await _httpClient.PostAsync($"{BaseAddress}reservations", new StringContent(body));
        }
    }
}
