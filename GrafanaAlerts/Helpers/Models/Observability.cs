namespace GrafanaAlerts.Helpers.Models
{
    public class Observability
    {
        public string TextLogsLocation { get; set; }
        
        // Jaeger settings
        public bool IsJaegerAgentEnabled { get; set; }
        public string JaegerAgentHost { get; set; }
        public int JaegerAgentPort { get; set; }
        
        // Graylog settings
        public bool IsGraylogEnabled { get; set; }
        public string GraylogHost { get; set; }
        public int GraylogPort { get; set; }
        public bool UseSsl { get; set; }
    }
}