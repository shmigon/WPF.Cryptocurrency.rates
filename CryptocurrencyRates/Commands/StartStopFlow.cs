using CryptocurrencyRates.Configuration;
using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using CryptocurrencyRates.VM;
using System;
using System.Collections.ObjectModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace CryptocurrencyRates.Commands
{
    public class StartStopFlow : IStartStopFlow
    {
        #region Fields
        private volatile bool _started = false;
        private DispatcherTimer _timer = new DispatcherTimer();

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
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(0);
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

                if (_timer.Interval == TimeSpan.FromSeconds(0))
                {
                    _timer.Stop();
                    _timer.Interval = TimeSpan.FromSeconds(_settings.TimerIntervalSec);
                    _timer.Start();
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
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
            else
            {
                _timer.Interval = TimeSpan.FromSeconds(0);
                _timer.Start();
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
