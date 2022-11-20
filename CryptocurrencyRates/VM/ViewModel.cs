using CryptocurrencyRates.Commands;
using CryptocurrencyRates.Configuration;
using CryptocurrencyRates.Services.Cryptocurrencies;
using CryptocurrencyRates.Services.WinServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CryptocurrencyRates.VM
{
    public class ViewModel : ViewModelBase
    {
        #region Static Readonly Fields
        public static readonly string START = "Start";
        public static readonly string STOP = "Stop";
        #endregion

        #region Private Fields
        private IStartStopFlow _startStopFlow;
        private string _buttonStartText = ViewModel.START;
        private string _servicesInfo;
        private ObservableCollection<CurrencyInfo> _currencyInfoCollection = new ObservableCollection<CurrencyInfo>();
        private readonly DelegateCommand _startStopCommand;
        #endregion

        #region Properties
        public string ButtonStartText
        {
            get => _buttonStartText;
            set => SetProperty(ref _buttonStartText, value);
        }

        
        public string ServicesInfo {
            get => _servicesInfo;
            set => SetProperty(ref _servicesInfo, value);
        }
        
        public ObservableCollection<CurrencyInfo> CurrencyInfoCollection { 
            get => _currencyInfoCollection; 
            set
            {
                SetProperty(ref _currencyInfoCollection, value);
            }
        }
        
        public ICommand StartStopCommand => _startStopCommand;

        public IStartStopFlow StartStopFlow { 
            get => _startStopFlow; 
            set { _startStopFlow = value; _startStopFlow.ViewModel = this; } 
        }
        #endregion

        #region Ctors
        public ViewModel()
        {
            _startStopCommand = new DelegateCommand(OnStartStop);
        }
        #endregion

        #region Command Events
        private void OnStartStop(object commandParameter)
        {
            if (_startStopFlow != null) _startStopFlow.Toggle();
        }
        #endregion
    }
}
