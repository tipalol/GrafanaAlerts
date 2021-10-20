using System;
using GrafanaAlerts.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

namespace GrafanaAlerts
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var config = ConfigHelper.Load();
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File($"{config.Observability.TextLogsLocation}log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console();

            if (config.Observability.IsGraylogEnabled)
                loggerConfiguration.WriteTo.Graylog(new GraylogSinkOptions()
                {
                    HostnameOrAddress = config.Observability.GraylogHost,
                    Port = config.Observability.GraylogPort,
                    TransportType = TransportType.Http,
                    UseSsl = config.Observability.UseSsl,
                });

            Log.Logger = loggerConfiguration.CreateLogger();
            
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}