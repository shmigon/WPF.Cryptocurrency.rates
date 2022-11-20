using System.Collections.ObjectModel;

namespace CryptocurrencyRates.VM
{
    public class ViewModel : ViewModelBase
    {
        private string _buttonStartText;
        public string ButtonStartText
        {
            get => _buttonStartText;
            set => SetProperty(ref _buttonStartText, value);
        }

        private string _servicesInfo;
        public string ServicesInfo {
            get => _servicesInfo;
            set => SetProperty(ref _servicesInfo, value);
        }

        private ObservableCollection<CurrencyInfo> _currencyInfoCollection = new ObservableCollection<CurrencyInfo>();
        public ObservableCollection<CurrencyInfo> CurrencyInfoCollection { 
            get => _currencyInfoCollection; 
            set
            {
                _currencyInfoCollection = value;
                SetProperty(ref _currencyInfoCollection, value);
            }
        }
    }
}
