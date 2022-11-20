using CryptocurrencyRates.Commands;
using CryptocurrencyRates.Configuration.IoC;
using CryptocurrencyRates.VM;
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

            var window = _kernel.Get<MainWindow>();
            
            var viewModel = new ViewModel();
            viewModel.StartStopFlow = _kernel.Get<IStartStopFlow>();

            window.DataContext = viewModel;

            Current.MainWindow = window;
            Current.MainWindow.Show();
        }
    }
}
