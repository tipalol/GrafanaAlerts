using System;
using System.Collections.Generic;
using GrafanaAlerts.API.Exceptions;
using GrafanaAlerts.API.Models;
using GrafanaAlerts.Core.Entities;
using GrafanaAlerts.Core.Enums;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.API.Services.Implementations
{
    internal sealed class GrafanaAlertParseService : IGrafanaAlertParseService
    {
        private const string RoleTag = "role";
        private const string KeTag = "ke";
        private const string PriorityTag = "priority";
        private const string InitiatorTypeTag = "initiatorType";
        private const string InitiatorRoleTag = "initiatorRole";

        private readonly ILogger<GrafanaAlertParseService> _logger;

        public GrafanaAlertParseService(ILogger<GrafanaAlertParseService> logger)
        {
            _logger = logger;
        }
        
        public TroubleTicket Parse(TriggerAlertRequest request)
        {
            var id = request.RuleId;
            
            var name = request.RuleName;
            ThrowIfNull(name, nameof(request.RuleName), request);
            
            var description = request.Message;
            ThrowIfNull(description, nameof(request.Message), request);
            
            var tags = request.Tags;
            ThrowIfNull(tags, nameof(request.Tags), request);

            if (tags.ContainsKey("Code"))
            {
                
            }

            try
            {
                var role = tags[RoleTag];
                var ke = tags[KeTag];
                var priority = tags[PriorityTag];
                var initiatorType = Convert.ToInt32(tags[InitiatorTypeTag]);
                var initiatorRole = tags[InitiatorRoleTag];
            
                ThrowIfNull(role, nameof(RoleTag), request);
                ThrowIfNull(ke, nameof(KeTag), request);
                ThrowIfNull(priority, nameof(PriorityTag), request);
                ThrowIfNull(initiatorType, nameof(InitiatorTypeTag), request);
                ThrowIfNull(initiatorRole, nameof(InitiatorRoleTag), request);

                var parsedInitiatorType = InitiatorType.Company;
                if (initiatorType == 0) parsedInitiatorType = InitiatorType.Person;

                var ticket = new TroubleTicket
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Ke = ke,
                    Role = role,
                    Priority = priority,
                    InitiatorType = parsedInitiatorType,
                    InitiatorRole = initiatorRole
                };
                
                return ticket;
            }
            catch (KeyNotFoundException exception)
            {
                _logger.LogError("Grafana request does not contain all tags needed.. " +
                                 "Exception: {@Exception}, Request: {@Request}", exception, request);
                
                throw new GrafanaAlertParseException("Grafana request does not contain all tags needed", request);
            }
        }
        
        private void ThrowIfNull(object nullable, string name, TriggerAlertRequest request)
        {
            if (nullable != null) return;
            
            _logger.LogError("Found empty property {Name} while processing request {@Request}", name, request);
            throw new GrafanaAlertParseException($"Empty property {name}", request);
        }
    }
}