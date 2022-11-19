using CryptocurrencyRates.IocConfiguration;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CryptocurrencyRates
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _kernel = new StandardKernel();
            _kernel.Load(new CryptocurrencyRatesModule());

            Current.MainWindow = _kernel.Get<MainWindow>();
            Current.MainWindow.Show();
        }
    }
}
