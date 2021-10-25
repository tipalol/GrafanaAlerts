using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GrafanaAlerts.API.Models
{
    /// <summary>
    /// Request body from Grafana when alarm triggered
    /// </summary>
    public class TriggerAlertRequest
    {
        [JsonPropertyName("dashboardId")]
        public int DashboardId { get; set; }
        [JsonPropertyName("evalMatches")]
        public EvaluationMatch[] EvalMatches { get; set; }
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("orgId")]
        public int OrganisationId { get; set; }
        [JsonPropertyName("panelId")]
        public int PanelId { get; set; }
        [JsonPropertyName("ruleId")]
        public int RuleId { get; set; }
        [JsonPropertyName("ruleName")]
        public string RuleName { get; set; }
        [JsonPropertyName("ruleUrl")]
        public string RuleUrl { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("tags")]
        public Dictionary<string, string> Tags { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}