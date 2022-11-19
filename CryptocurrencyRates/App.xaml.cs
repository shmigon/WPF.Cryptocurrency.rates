using CryptocurrencyRates.Configuration.IoC;
using Ninject;
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
