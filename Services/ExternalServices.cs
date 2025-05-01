using dotnet_simple_bank.Interfaces;

namespace dotnet_simple_bank.Services
{
    public class ExternalServices(HttpClient httpClient) : IExternalServices
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> AuthTransferAsync()
        {
            var response = await _httpClient.GetAsync("https://util.devi.tools/api/v2/authorize");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> MessageTransferReceivedAsync()
        {
            var response = await _httpClient.PostAsync("https://util.devi.tools/api/v1/notify", null);

            return response.IsSuccessStatusCode;
        }
    }
}
