namespace GrafanaAlerts.Infrastructure.Configuration.Models
{
    public class App
    {
        public bool AllowCustomProperties { get; set; }
        public string SoapRequestsLocation { get; set; }
        public string TroubleTicketSystemHost { get; set; }
        public string DatabaseConnectionString { get; set; }
    }
}