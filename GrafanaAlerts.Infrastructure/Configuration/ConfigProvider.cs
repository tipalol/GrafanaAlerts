using System.IO;
using System.Text.Json;
using GrafanaAlerts.Infrastructure.Configuration.Models;

namespace GrafanaAlerts.Infrastructure.Configuration
{
    public class ConfigProvider
    {
        private static Config _config;

        public static Config Load()
        {
            if (_config != null)
                return _config;

            var json = File.ReadAllText("AppData/Configs/config.json");
            _config = JsonSerializer.Deserialize<Config>(json);

            return _config;
        }
    }
}