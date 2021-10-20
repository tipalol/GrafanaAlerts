namespace GrafanaAlerts.Models
{
    public class TroubleTicket
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ke { get; set; }
        public string Role { get; set; }
        public string Priority { get; set; }
        public string InitiatorType { get; set; }
        public string InitiatorRole {get;set;}
    }
}