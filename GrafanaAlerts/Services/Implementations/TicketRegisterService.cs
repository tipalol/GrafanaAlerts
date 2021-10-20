using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GrafanaAlerts.Enums;
using GrafanaAlerts.Helpers;
using GrafanaAlerts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Services.Implementations
{
    internal sealed class TicketRegisterService : ITicketRegisterService, IDisposable
    {
        private const string GuidTag = "GUID";
        private const string IdTag = "ID";
        private const string DatetimeTag = "DateTime";
        private const string DescriptionTag = "AlertDescription";
        private const string NameTag = "AlertName";
        private const string KeTag = "KE";
        private const string RoleTag = "Role";
        private const string PriorityTag = "Priority";
        private const string OperatorTag = "Operator";
        private const string InitiatorTypeTag = "InitiatorType";
        private const string InitiatorRoleTag = "InitiatorRole";
        
        private const string DefaultRole = "ROL000000000388";
        private const string DefaultPriority = "PRI000000000006";
        private const string DefaultOperator = "BeeInside";
        private const string DefaultInitiatorRole = "BeeInside";
        private const InitiatorType DefaultInitiatorType = InitiatorType.Company;

        private const string CreateTroubleTicketRequest = "CreateTTRequest";
        
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
            var rawRequest = _requestProvider.GetRequest(CreateTroubleTicketRequest);

            var id = EnvironmentHelper.AddTicket();
            _logger.LogInformation("There is {Id} tickets in system", id);

            var role = _isCustomAllowed ? ticket.Role : DefaultRole;
            var priority = _isCustomAllowed ? ticket.Priority : DefaultPriority;
            var initiatorType = _isCustomAllowed ? ticket.InitiatorType : DefaultInitiatorType;
            var initiatorRole = ticket.InitiatorType == InitiatorType.Person ? DefaultInitiatorRole : ticket.InitiatorRole;

            var request = new RequestBuilder(rawRequest)
                .SetAttribute(GuidTag, Guid.NewGuid().ToString())
                .SetAttribute(IdTag, $"{id}")
                .SetAttribute(DatetimeTag, DateTime.Now.ToString("O"))
                .SetAttribute(DescriptionTag, ticket.Description)
                .SetAttribute(NameTag, ticket.Name)
                .SetAttribute(KeTag, ticket.Ke)
                .SetAttribute(RoleTag,  role)
                .SetAttribute(PriorityTag, priority)
                .SetAttribute(OperatorTag, DefaultOperator)
                .SetAttribute(InitiatorTypeTag, $"{(int)initiatorType}")
                .SetAttribute(InitiatorRoleTag, initiatorRole)
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