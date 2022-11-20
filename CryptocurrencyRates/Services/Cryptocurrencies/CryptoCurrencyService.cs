using CryptocurrencyRates.Configuration;
using CryptocurrencyRates.VM;
using Newtonsoft.Json;
using System.Collections.Generic;
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
            dynamic res = JsonConvert.DeserializeObject(rates);
            return res;
        }

        public async Task<List<CurrencyInfo>> GetRateListAsync()
        {
            dynamic res = await GetRatesAsync();
            var result = new List<CurrencyInfo>();
            if (res != null && res.data != null)
            {
                foreach (var rateData in res.data)
                {
                    result.Add(new CurrencyInfo() { CurrencyName = rateData.name, PriceUsd = rateData.priceUsd });
                }
            }
            return result;
        }
    }
}
