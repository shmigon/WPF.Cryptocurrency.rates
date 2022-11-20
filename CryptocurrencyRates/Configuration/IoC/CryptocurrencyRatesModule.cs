using CryptocurrencyRates.Commands;
using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using Ninject.Modules;

namespace CryptocurrencyRates.Configuration.IoC
{
    public class CryptocurrencyRatesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>().InSingletonScope();

            Bind<ICryptoCurrencyService>().To<CryptoCurrencyService>().InSingletonScope();
            Bind<IWinServicesService>().To<WinServicesService>().InSingletonScope();
            Bind<IStartStopFlow>().To<StartStopFlow>().InSingletonScope();
        }
    }
}
