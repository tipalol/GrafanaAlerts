using System.Threading.Tasks;
using GrafanaAlerts.Models;

namespace GrafanaAlerts.Services.Implementations
{
    internal sealed class AdditionalTroubleInformationProviderService : IAdditionalTroubleInformationProviderService
    {
        public async Task<TroubleTicket> Complete(TroubleTicket ticket)
        {
            await Task.CompletedTask;
            
            return ticket;
        }
    }
}