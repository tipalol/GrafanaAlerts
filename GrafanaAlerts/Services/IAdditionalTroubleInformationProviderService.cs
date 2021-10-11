using System.Threading.Tasks;
using GrafanaAlerts.Models;

namespace GrafanaAlerts.Services
{
    public interface IAdditionalTroubleInformationProviderService
    {
        public Task<TroubleTicket> Complete(TroubleTicket ticket);
    }
}