using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptocurrencyRates.VM
{
    public class CurrencyInfo : ViewModelBase
    {
        private string _currencyName;
        public string CurrencyName { 
            get => _currencyName; 
            set => SetProperty(ref _currencyName, value); 
        }

        private double _priceUsd;
        public double PriceUsd
        {
            get => _priceUsd;
            set => SetProperty(ref _priceUsd, value);
        }
    }
}
