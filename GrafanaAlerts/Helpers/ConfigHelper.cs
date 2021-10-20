using System.IO;
using System.Text.Json;
using GrafanaAlerts.Helpers.Models;

namespace GrafanaAlerts.Helpers
{
    public class ConfigHelper
    {
        private static Config _config;

        public static Config Load()
        {
            if (_config != null)
                return _config;

            var json = File.ReadAllText("AppData/Configs/Config.json");
            _config = JsonSerializer.Deserialize<Config>(json);

            return _config;
        }
    }
}