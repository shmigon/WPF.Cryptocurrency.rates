using System.Threading.Tasks;

namespace CryptocurrencyRates.Services.Cryptocurrencies
{
    public interface ICryptoCurrencyService
    {
        Task<dynamic> GetRatesAsync();
    }
}
