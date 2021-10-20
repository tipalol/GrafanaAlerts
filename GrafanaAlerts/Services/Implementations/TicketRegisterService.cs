using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GrafanaAlerts.Helpers;
using GrafanaAlerts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Services.Implementations
{
    internal sealed class TicketRegisterService : ITicketRegisterService, IDisposable
    {
        private const string DefaultRole = "ROL000000000388";
        private const string DefaultPriority = "PRI000000000006";
        private const string DefaultOperator = "BeeInside";
        private const string DefaultInitiatorType = "1";
        
        private readonly HttpClient _client;
        private readonly bool _isCustomAllowed;
        private readonly string _troubleTicketSystemHost;

        private readonly IRequestProviderService _requestProvider;
        private readonly ILogger<TicketRegisterService> _logger;

        public TicketRegisterService(IRequestProviderService requestProvider, ILogger<TicketRegisterService> logger)
        {
            _client = new HttpClient();
            var config = ConfigHelper.Load();
            _troubleTicketSystemHost = config.App.TroubleTicketSystemHost;
            _isCustomAllowed = config.App.AllowCustomProperties;
            _requestProvider = requestProvider;
            _logger = logger;
        }

        public async Task<HttpStatusCode> Register(TroubleTicket ticket)
        {
            _logger.LogInformation("Loading CreateTTRequest.xml...");
            var rawRequest = _requestProvider.GetRequest("CreateTTRequest");

            var id = EnvironmentHelper.GetTicketsCount();
            _logger.LogInformation("There is {Id} tickets in system. Incrementing..", id);
            id = EnvironmentHelper.AddTicket();

            var role = _isCustomAllowed ? ticket.Role : DefaultRole;
            var priority = _isCustomAllowed ? ticket.Priority : DefaultPriority;
            var initiatorType = _isCustomAllowed ? ticket.InitiatorType : DefaultInitiatorType;
            var initiatorRole = ticket.InitiatorType == "0" ? DefaultOperator : ticket.InitiatorRole;

            var request = new RequestBuilder(rawRequest)
                .SetAttribute("GUID", Guid.NewGuid().ToString())
                .SetAttribute("ID", $"{id}")
                .SetAttribute("DateTime", DateTime.Now.ToString("O"))
                .SetAttribute("AlertDescription", ticket.Description)
                .SetAttribute("AlertName", ticket.Name)
                .SetAttribute("KE", ticket.Ke)
                .SetAttribute("Role",  role)
                .SetAttribute("Priority", priority)
                .SetAttribute("Operator", DefaultOperator)
                .SetAttribute("InitiatorType", initiatorType)
                .SetAttribute("InitiatorRole", initiatorRole)
                .Build();
            
            _logger.LogInformation("After changing attributes request is {@Request}", request);
            
            _logger.LogInformation("Sending soap request to the Ticket system");
            var response = await SoapHelper.SendRequest(_troubleTicketSystemHost, request);
            
            _logger.LogInformation("Got response from Ticket System: @{Response}", response);

            return HttpStatusCode.OK;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}