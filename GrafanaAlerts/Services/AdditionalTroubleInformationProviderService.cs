using System.Threading.Tasks;
using GrafanaAlerts.Exceptions;
using GrafanaAlerts.Models;

namespace GrafanaAlerts.Services
{
    internal sealed class AdditionalTroubleInformationProviderService : IAdditionalTroubleInformationProviderService
    {
        public async Task<TroubleTicket> Complete(TroubleTicket ticket)
        {
            await Task.Delay(500);

            return ticket;
            //throw new ServiceNotRespondingException("Service is not available", "Some service");
        }
    }
}