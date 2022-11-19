using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptocurrencyRates.Services.Cryptocurrencies
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private HttpClient _httpClient;

        public CryptoCurrencyService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<dynamic> GetRatesAsync()
        {

            // Here we filter the cryptocurrency list directly through the API. 
            // In a real app move the ids to the configuration (for instance to the config file App.config)
            Task<string> getTask = _httpClient.GetStringAsync("https://api.coincap.io/v2/assets?ids=bitcoin,ethereum,dogecoin");

            string rates = await getTask;

            // Here we use "dynamic" for the sake of simplicity.
            // Just not to have an entity class with INotifyPropertyChanged interface implementation.
            dynamic res = JsonConvert.DeserializeObject(rates);
            return res;
        }
    }
}
