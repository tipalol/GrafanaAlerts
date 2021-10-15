using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GrafanaAlerts.Helpers;
using GrafanaAlerts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Services
{
    internal sealed class TicketRegisterService : ITicketRegisterService, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _troubleTicketSystemHost;

        private readonly IRequestProviderService _requestProvider;
        private readonly ILogger<TicketRegisterService> _logger;

        public TicketRegisterService(IConfiguration configuration, IRequestProviderService requestProvider, ILogger<TicketRegisterService> logger)
        {
            _client = new HttpClient();
            _troubleTicketSystemHost = configuration["TroubleTicketSystemHost"];
            _requestProvider = requestProvider;
            _logger = logger;
        }

        private string RegisterTicketApiMethod => $"{_troubleTicketSystemHost}";
        
        public async Task<HttpStatusCode> Register(TroubleTicket ticket)
        {
            _logger.LogInformation("Getting CreateTTRequest...");
            var rawRequest = _requestProvider.GetRequest("CreateTTRequest");
            _logger.LogInformation("Got request {@Request}", rawRequest);

            var id = EnvironmentHelper.GetTicketsCount();
            _logger.LogInformation("There is {Id} tickets in system. Incrementing..", id);
            id = EnvironmentHelper.AddTicket();

            var request = new RequestBuilder(rawRequest)
                .ChangeAttribute("GUID", Guid.NewGuid().ToString())
                .ChangeAttribute("Id", $"GRAFANA{id}")
                .ChangeAttribute("DateTime", DateTime.Now.ToString("O"))
                .ChangeAttribute("AlertDescription", ticket.Description)
                .ChangeAttribute("AlertName", ticket.Name)
                .ChangeAttribute("KE", ticket.Ke)
                .Build();
            
            _logger.LogInformation("After changing attributes request is {@Request}", request);
            
            _logger.LogInformation("Sending soap request to the Ticket system");
            var response = await SoapHelper.SendRequest(RegisterTicketApiMethod, request);
            
            _logger.LogInformation("Got response from Ticket System: @{Response}", response);

            return HttpStatusCode.OK;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}