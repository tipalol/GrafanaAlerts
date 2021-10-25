using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;
using GrafanaAlerts.Core.Services;

namespace GrafanaAlerts.Infrastructure.Services
{
    public class AdditionalTroubleInformationProviderService : IAdditionalTroubleInformationProviderService
    {
        public async Task<TroubleTicket> Complete(TroubleTicket ticket)
        {
            await Task.CompletedTask;
            
            return ticket;
        }
    }
}