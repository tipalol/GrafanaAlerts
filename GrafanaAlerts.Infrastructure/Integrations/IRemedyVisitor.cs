using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;

namespace GrafanaAlerts.Infrastructure.Integrations
{
    public interface IRemedyVisitor
    {
        public Task<string> AcceptTroubleTicket(TroubleTicket ticket);
    }
}