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

        private readonly int SECONDS = 10;
        #endregion

        #region private fields
        private volatile bool started = false;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private HttpClient httpClient = new HttpClient();
        #endregion

        #region Ctors
        public MainWindow()
        {
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
            if (started) return;

            try
            {
                started = true;

                Task taskFillRates = FillRates();
                Task taskFillServices = Task.Run(() => FillServices());

                await Task.WhenAll(taskFillRates, taskFillServices);

                if (timer.Interval == TimeSpan.FromSeconds(0))
                {
                    timer.Stop();
                    timer.Interval = TimeSpan.FromSeconds(SECONDS);
                    timer.Start();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //TODO: Log exception

                //TODO: another try catch?
                Toggle();
                
            }
            started = false;
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
            // Here we filter the cryptocurrency list directly through the API. 
            // In a real app move the ids to the configuration (for instance to the config file App.config)
            Task<string> getTask = httpClient.GetStringAsync("https://api.coincap.io/v2/assets?ids=bitcoin,ethereum,dogecoin");

            string rates = await getTask;

            // Here we use "dynamic" for the sake of simplicity.
            // Just not to have an entity class with INotifyPropertyChanged interface implementation.
            dynamic res = Newtonsoft.Json.JsonConvert.DeserializeObject(rates);
            if (!string.IsNullOrWhiteSpace(rates) && res != null && res.data != null && started)
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
            var services = ServiceController.GetServices()
                .Where(s => s.Status == ServiceControllerStatus.Running);

            if (!started) return;

            Dispatcher.Invoke(new Action(() =>
            {

                txtMultiline.Text = String.Empty;
                foreach (var service in services)
                {
                    txtMultiline.AppendText($"{service.ServiceName} - {service.DisplayName}\n");
                }
            }));
        }
        #endregion
    }
}
