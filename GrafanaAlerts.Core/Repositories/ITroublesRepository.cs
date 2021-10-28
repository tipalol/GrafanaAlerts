using System.Net;
using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;

namespace GrafanaAlerts.Core.Repositories
{
    public interface ITroublesRepository
    {
        public Task<HttpStatusCode> Add(TroubleTicket ticket);
        public Task<HttpStatusCode> Close(int ticketId);
    }
}