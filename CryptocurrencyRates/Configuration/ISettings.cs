namespace CryptocurrencyRates.Configuration
{
    public interface ISettings
    {
        string CryptoApiUrl { get; }
        string CryptoAssetIds { get; }
        int TimerIntervalSec { get; }
    }
}
