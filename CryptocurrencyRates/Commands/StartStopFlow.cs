using CryptocurrencyRates.Configuration;
using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using CryptocurrencyRates.VM;
using System;
using System.Collections.ObjectModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;

namespace CryptocurrencyRates.Commands
{
    public class StartStopFlow : IStartStopFlow
    {
        #region Fields
        private volatile bool _started = false;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private ISettings _settings;
        private ICryptoCurrencyService _cryptoCurrencyService;
        private IWinServicesService _winServicesService;

        private ViewModel _viewModel;
        #endregion

        #region Properties
        public ViewModel ViewModel { get => _viewModel; set => _viewModel = value; }
        #endregion

        #region Ctors
        public StartStopFlow(
            ISettings settings,
            ICryptoCurrencyService cryptoCurrencyService,
            IWinServicesService winServicesService)
        {
            _settings = settings;
            _cryptoCurrencyService = cryptoCurrencyService;
            _winServicesService = winServicesService;
            InitTimer();
        }
        #endregion

        #region Timer methods
        private void InitTimer()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(0);
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (_started || _viewModel == null) return;
            try
            {
                _started = true;

                Task taskFillRates = FillRates();
                Task taskFillServices = Task.Run(() => FillServices());

                await Task.WhenAll(taskFillRates, taskFillServices);

                if (timer.Interval == TimeSpan.FromSeconds(0))
                {
                    timer.Stop();
                    timer.Interval = TimeSpan.FromSeconds(_settings.TimerIntervalSec);
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //TODO: Log exception

                //TODO: another try catch?
                Toggle();

            }
            _started = false;
        }
        #endregion

        #region IStartStopFlow.Toggle
        public void Toggle()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Interval = TimeSpan.FromSeconds(0);
                timer.Start();
            }
            ViewModel.ButtonStartText = (ViewModel.ButtonStartText == ViewModel.START) ? ViewModel.STOP : ViewModel.START;
        }
        #endregion

        #region Fill data methods
        private async Task FillRates()
        {
            ViewModel.CurrencyInfoCollection = new ObservableCollection<CurrencyInfo>(
                await _cryptoCurrencyService.GetRateListAsync());
        }

        private void FillServices()
        {
            string info = _winServicesService.GetServicesInfo(ServiceControllerStatus.Running);
            ViewModel.ServicesInfo = info;
        }
        #endregion
    }
}
