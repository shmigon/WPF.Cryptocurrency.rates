using CryptocurrencyRates.Configuration;
using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptocurrencyRates
{
    /* For the simplicity all the logic is within this class.
     * According to SOLID principles real app should be implemented respectivelly. :)
     */ 

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region readonly fields
        private readonly string START = "Start";
        private readonly string STOP = "Stop";

        private readonly string HEADER_CURRENCY = "Currency name";
        private readonly string HEADER_PRICE = "Price in USD";

        private readonly string PROP_CURRENCY= "name";
        private readonly string PROP_PRICE = "priceUsd";

        #endregion

        #region private fields
        private volatile bool _started = false;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        private ISettings _settings;
        private ICryptoCurrencyService _cryptoCurrencyService;
        private IWinServicesService _winServicesService;
        #endregion

        #region Ctors
        public MainWindow(ISettings settings,
            ICryptoCurrencyService cryptoCurrencyService, 
            IWinServicesService winServicesService)
        {
            this._settings = settings;
            this._cryptoCurrencyService = cryptoCurrencyService;
            this._winServicesService = winServicesService;

            InitializeComponent();
            InitGrid();
            InitTimer();
        }
        #endregion

        #region Initialization
        private void InitTimer()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(0);
        }

        private void InitGrid()
        {
            gridRates.Columns.Add(new DataGridTextColumn() { Header = HEADER_CURRENCY, Binding = new Binding(PROP_CURRENCY) });
            gridRates.Columns.Add(new DataGridTextColumn() { Header = HEADER_PRICE, Binding = new Binding(PROP_PRICE) });
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
            btnStart.Content = (btnStart.Content.ToString() == START) ? STOP : START;
        }
        #endregion

        #region Fill data methods
        private async Task FillRates()
        {
            dynamic res = await _cryptoCurrencyService.GetRatesAsync();
            if (res != null && res.data != null && _started)
            {
                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    gridRates.Items.Clear();
                    foreach (var rateData in res.data)
                    {
                        gridRates.Items.Add(rateData);
                    }
                }));
            }
        }

        private void FillServices()
        {
            string info = _winServicesService.GetServicesInfo(ServiceControllerStatus.Running);
            Dispatcher.Invoke(new Action(() =>
            {
                txtMultiline.Text = info;
            }));
        }
        #endregion
    }
}
