using GrafanaAlerts.Core.Enums;

namespace GrafanaAlerts.Core.Entities
{
    public class TroubleTicket
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ke { get; set; }
        public string Role { get; set; }
        public string Priority { get; set; }
        public InitiatorType InitiatorType { get; set; }
        public string InitiatorRole {get;set;}
    }
}