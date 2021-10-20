using System.Collections.Generic;
using GrafanaAlerts.Exceptions;
using GrafanaAlerts.Models;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Services.Implementations
{
    internal sealed class GrafanaAlertParseService : IGrafanaAlertParseService
    {
        private const string RoleTag = "role";
        private const string KeTag = "ke";
        private const string PriorityTag = "priority";

        private readonly ILogger<GrafanaAlertParseService> _logger;

        public GrafanaAlertParseService(ILogger<GrafanaAlertParseService> logger)
        {
            _logger = logger;
        }
        
        public TroubleTicket Parse(TriggerAlertRequest request)
        {
            TroubleTicket ticket;

            var name = request.RuleName;
            ThrowIfNull(name, nameof(request.RuleName), request);
            
            var description = request.Message;
            ThrowIfNull(description, nameof(request.Message), request);
            
            var tags = request.Tags;
            ThrowIfNull(tags, nameof(request.Tags), request);

            try
            {
                var role = tags[RoleTag];
                var ke = tags[KeTag];
                var priority = tags[PriorityTag];
            
                ThrowIfNull(role, nameof(RoleTag), request);
                ThrowIfNull(ke, nameof(KeTag), request);
                ThrowIfNull(priority, nameof(PriorityTag), request);

                ticket = new TroubleTicket
                {
                    Name = name,
                    Description = description,
                    Ke = ke,
                    Role = role,
                    Priority = priority
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