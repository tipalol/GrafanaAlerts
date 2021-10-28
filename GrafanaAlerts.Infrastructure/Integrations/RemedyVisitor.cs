using System;
using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;
using GrafanaAlerts.Core.Enums;
using GrafanaAlerts.Core.Repositories;
using GrafanaAlerts.Infrastructure.Builders;
using GrafanaAlerts.Infrastructure.Configuration;
using GrafanaAlerts.Infrastructure.DTO;
using GrafanaAlerts.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Infrastructure.Integrations
{
    public class RemedyVisitor : IRemedyVisitor
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
        private const string SolveTroubleTicketRequest = "SolveTTRequest";
        
        private readonly bool _isCustomAllowed;
        private readonly string _troubleTicketSystemHost;

        private readonly IRequestRepository _requestRepository;
        private readonly ILogger<RemedyVisitor> _logger;

        public RemedyVisitor(IRequestRepository requestRepository, ILogger<RemedyVisitor> logger)
        {
            var config = ConfigProvider.Load();
            _troubleTicketSystemHost = config.App.TroubleTicketSystemHost;
            _isCustomAllowed = config.App.AllowCustomProperties;
            _requestRepository = requestRepository;
            _logger = logger;
        }

        public async Task<string> Accept(TroubleTicket ticket)
        {
            _logger.LogInformation("Loading CreateTTRequest.xml...");
            var rawRequest = _requestRepository.Get(CreateTroubleTicketRequest);

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
            
            _logger.LogInformation("Request is {@Request}", request);
            
            _logger.LogInformation("Sending soap request to the Ticket system..");
            var response = await SoapHelper.SendRequest(_troubleTicketSystemHost, request);
            
            _logger.LogInformation("Got response from Ticket system: @{Response}", response);

            var registeredId = SoapHelper.GetTroubleId(response);
            
            _logger.LogInformation("Trouble id is {Id}", registeredId);

            return registeredId;
        }

        public async Task<string> Close(TroubleTicketDTO ticket)
        {
            _logger.LogInformation("Loading {Request}.xml...", SolveTroubleTicketRequest);
            var rawRequest = _requestRepository.Get(SolveTroubleTicketRequest);

            var id = ticket.TroubleId;
            var guid = Guid.NewGuid().ToString();
            var date = DateTime.Now.ToString("O");

            var request = new RequestBuilder(rawRequest)
                .SetAttribute(IdTag, id)
                .SetAttribute(GuidTag, guid)
                .SetAttribute(DatetimeTag, date)
                .Build();
            
            _logger.LogInformation("Request is {@Request}", request);
            _logger.LogInformation("Sending soap request to the Ticket system..");
            
            var response = await SoapHelper.SendRequest(_troubleTicketSystemHost, request);
            
            _logger.LogInformation("Got response from Ticket system: @{Response}", response);

            return response;
        }
    }
}