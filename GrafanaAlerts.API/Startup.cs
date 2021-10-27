using GrafanaAlerts.API.Controllers;
using GrafanaAlerts.API.Services;
using GrafanaAlerts.API.Services.Implementations;
using GrafanaAlerts.Core.Repositories;
using GrafanaAlerts.Core.Services;
using GrafanaAlerts.Infrastructure.Configuration;
using GrafanaAlerts.Infrastructure.Integrations;
using GrafanaAlerts.Infrastructure.Repositories;
using GrafanaAlerts.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

namespace GrafanaAlerts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var config = ConfigProvider.Load();
            
            if (config.Observability.IsJaegerAgentEnabled)
            {
                var jaegerAgentHost = config.Observability.JaegerAgentHost;
            
                services.AddOpenTelemetryTracing(
                    builder => builder
                        .AddSource(nameof(GrafanaController))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter()
                        .AddJaegerExporter(options => options.AgentHost = jaegerAgentHost)
                );
            }
            else
            {
                services.AddOpenTelemetryTracing(
                    builder => builder
                        .AddSource(nameof(GrafanaController))
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter()
                );
            }
            
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "GrafanaAlerts.API", Version = "v1"});
            });

            services.AddSingleton<IGrafanaAlertParseService, GrafanaAlertParseService>();
            services.AddSingleton<IAdditionalTroubleInformationProviderService, 
                AdditionalTroubleInformationProviderService>();
            services.AddSingleton<ITroublesRepository, TroublesRepository>();
            services.AddSingleton<IRequestRepository, RequestsRepository>();
            services.AddSingleton<IRemedyVisitor, RemedyVisitor>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrafanaAlerts.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}