using CryptocurrencyRates.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptocurrencyRates.Services.Cryptocurrencies
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private HttpClient _httpClient;
        private ISettings _settings;

        public CryptoCurrencyService(ISettings settings)
        {
            _httpClient = new HttpClient();
            _settings = settings;
        }

        public async Task<dynamic> GetRatesAsync()
        {
            var apiUrl = string.IsNullOrWhiteSpace(_settings.CryptoAssetIds) ? _settings.CryptoApiUrl : 
                $"{_settings.CryptoApiUrl}?ids={_settings.CryptoAssetIds}";

            Task<string> getTask = _httpClient.GetStringAsync(apiUrl);

            string rates = await getTask;

            // Here we use "dynamic" for the sake of simplicity.
            dynamic res = JsonConvert.DeserializeObject(rates);
            return res;
        }
    }
}
