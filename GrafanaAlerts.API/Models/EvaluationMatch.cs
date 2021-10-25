using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GrafanaAlerts.API.Models
{
    /// <summary>
    /// Object model included in TriggerAlertRequest as evalMatches
    /// </summary>
    public class EvaluationMatch
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("metric")]
        public string Metric { get; set; }
        [JsonPropertyName("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }
}