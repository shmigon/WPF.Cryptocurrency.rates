using CryptocurrencyRates.VM;

namespace CryptocurrencyRates.Commands
{
    public interface IStartStopFlow
    {
        ViewModel ViewModel { get; set; }
        void Toggle();
    }
}
