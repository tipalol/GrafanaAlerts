using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;
using GrafanaAlerts.Infrastructure.DTO;

namespace GrafanaAlerts.Infrastructure.Integrations
{
    public interface IRemedyVisitor
    {
        public Task<string> Accept(TroubleTicket ticket);
        public Task<string> Close(TroubleTicketDTO ticket);
    }
}