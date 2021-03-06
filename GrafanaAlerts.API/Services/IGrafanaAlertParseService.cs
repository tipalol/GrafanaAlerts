using GrafanaAlerts.API.Models;
using GrafanaAlerts.Core.Entities;

namespace GrafanaAlerts.API.Services
{
    public interface IGrafanaAlertParseService
    {
        /// <summary>
        /// Parse a Grafana alarm request to trouble ticket format
        /// </summary>
        /// <param name="request">Grafana alert request</param>
        /// <returns>Trouble ticket</returns>
        public TroubleTicket Parse(TriggerAlertRequest request);
    }
}