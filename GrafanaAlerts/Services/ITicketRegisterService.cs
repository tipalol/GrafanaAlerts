using System.Net;
using System.Threading.Tasks;
using GrafanaAlerts.Models;

namespace GrafanaAlerts.Services
{
    public interface ITicketRegisterService
    {
        public Task<HttpStatusCode> Register(TroubleTicket ticket);
    }
}