using GrafanaAlerts.Controllers;
using GrafanaAlerts.Services;
using GrafanaAlerts.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Trace;

namespace GrafanaAlerts
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
            var jaegerAgentHost = Configuration["JaegerAgentHost"];
            
            services.AddOpenTelemetryTracing(
                builder => builder
                    .AddSource(nameof(GrafanaController))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter()
                    .AddJaegerExporter(options => options.AgentHost = jaegerAgentHost)
                );
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "GrafanaAlerts", Version = "v1"});
            });

            services.AddSingleton<IGrafanaAlertParseService, GrafanaAlertParseService>();
            services.AddSingleton<IAdditionalTroubleInformationProviderService, 
                AdditionalTroubleInformationProviderService>();
            services.AddSingleton<ITicketRegisterService, TicketRegisterService>();
            services.AddSingleton<IRequestProviderService, RequestProviderService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrafanaAlerts v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}