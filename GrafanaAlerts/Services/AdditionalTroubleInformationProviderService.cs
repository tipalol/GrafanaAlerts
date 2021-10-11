using System.Threading.Tasks;
using GrafanaAlerts.Exceptions;
using GrafanaAlerts.Models;

namespace GrafanaAlerts.Services
{
    internal sealed class AdditionalTroubleInformationProviderService : IAdditionalTroubleInformationProviderService
    {
        public Task<TroubleTicket> Complete(TroubleTicket ticket)
        {
            throw new ServiceNotRespondingException("Service is not available", "Some service");
        }
    }
}