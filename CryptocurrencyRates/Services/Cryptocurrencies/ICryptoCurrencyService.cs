using CryptocurrencyRates.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptocurrencyRates.Services.Cryptocurrencies
{
    public interface ICryptoCurrencyService
    {
        Task<dynamic> GetRatesAsync();
        Task<List<CurrencyInfo>> GetRateListAsync();
    }
}
