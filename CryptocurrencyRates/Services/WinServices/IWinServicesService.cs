using System.Collections.Generic;
using System.ServiceProcess;

namespace CryptocurrencyRates.Services.WinServices
{
    public interface IWinServicesService
    {
        IEnumerable<ServiceController> GetServices(params ServiceControllerStatus[] statuses);

        string GetServicesInfo(params ServiceControllerStatus[] statuses);
    }
}
