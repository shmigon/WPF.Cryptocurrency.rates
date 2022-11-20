using CryptocurrencyRates.Configuration;
using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using CryptocurrencyRates.VM;
using System;
using System.Collections.ObjectModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;

namespace CryptocurrencyRates
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region readonly fields
        private readonly string START = "Start";
        private readonly string STOP = "Stop";
        #endregion

        #region private fields
        private volatile bool _started = false;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private ISettings _settings;
        private ICryptoCurrencyService _cryptoCurrencyService;
        private IWinServicesService _winServicesService;
        private ViewModel _viewModel;
        #endregion

        #region Ctors
        public MainWindow(ISettings settings,
            ICryptoCurrencyService cryptoCurrencyService, 
            IWinServicesService winServicesService)
        {
            this._settings = settings;
            this._cryptoCurrencyService = cryptoCurrencyService;
            this._winServicesService = winServicesService;

            _viewModel = new ViewModel() { ButtonStartText = START };
            DataContext = _viewModel;

            InitializeComponent();
            InitTimer();
        }
        #endregion

        #region Initialization
        private void InitTimer()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(0);
        }
        #endregion

        #region Timer task
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (_started) return;
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
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //TODO: Log exception

                //TODO: another try catch?
                Toggle();
                
            }
            _started = false;
        }
        #endregion

        #region Event Handlers
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Toggle();
        }
        #endregion

        #region Private Methods
        private void Toggle()
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
            _viewModel.ButtonStartText = (_viewModel.ButtonStartText == START) ? STOP : START;
        }
        #endregion

        #region Fill data methods
        private async Task FillRates()
        {
            _viewModel.CurrencyInfoCollection = new ObservableCollection<CurrencyInfo>(
                await _cryptoCurrencyService.GetRateListAsync());
        }

        private void FillServices()
        {
            string info = _winServicesService.GetServicesInfo(ServiceControllerStatus.Running);
            _viewModel.ServicesInfo = info;
        }
        #endregion
    }
}
