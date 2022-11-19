using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRates.IocConfiguration
{
    public class CryptocurrencyRatesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICryptoCurrencyService>().To<CryptoCurrencyService>().InSingletonScope();
            Bind<IWinServicesService>().To<WinServicesService>().InSingletonScope();
        }
    }
}
