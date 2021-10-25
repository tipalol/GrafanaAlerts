using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;

namespace GrafanaAlerts.Core.Services
{
    public interface IAdditionalTroubleInformationProviderService
    {
        public Task<TroubleTicket> Complete(TroubleTicket ticket);
    }
}