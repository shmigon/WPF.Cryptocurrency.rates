using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRates.Configuration.IoC
{
    public class CryptocurrencyRatesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>().InSingletonScope();

            Bind<ICryptoCurrencyService>().To<CryptoCurrencyService>().InSingletonScope();
            Bind<IWinServicesService>().To<WinServicesService>().InSingletonScope();
        }
    }
}
