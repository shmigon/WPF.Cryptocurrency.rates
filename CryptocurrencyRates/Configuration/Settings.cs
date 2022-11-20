using System;
using System.Configuration;

namespace CryptocurrencyRates.Configuration
{
    public class Settings : ISettings
    {
        private string _cryptoApiUrl;
        private string _cryptoAssetIds;
        private int _timerIntervalSec;

        public string CryptoApiUrl
        {
            get => _cryptoApiUrl;
        }

        public string CryptoAssetIds
        {
            get => _cryptoAssetIds;
        }

        public int TimerIntervalSec
        {
            get => _timerIntervalSec;
        }

        string ISettings.CryptoAssetIds => _cryptoAssetIds;

        public Settings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            _cryptoApiUrl = appSettings["crypto_api_url"];
            _cryptoAssetIds = appSettings["crypto_asset_ids"];
            _timerIntervalSec = Convert.ToInt32(appSettings["timer_interval_sec"]);
        }
    }
}
