using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace CryptocurrencyRates.Services.WinServices
{
    public class WinServicesService : IWinServicesService
    {
        public IEnumerable<ServiceController> GetServices(params ServiceControllerStatus[] statuses)
        {
            var result = ServiceController.GetServices()
                .Where(s => statuses.Contains(s.Status));
            return result;
        }
        public string GetServicesInfo(params ServiceControllerStatus[] statuses)
        {
            IEnumerable<ServiceController> services = GetServices(statuses);
            var sb = new StringBuilder();
            foreach (var service in services)
            {
                sb.Append($"{service.ServiceName} - {service.DisplayName}\n");
            }
            return sb.ToString();
        }
    }
}
