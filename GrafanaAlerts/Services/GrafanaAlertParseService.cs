using GrafanaAlerts.Exceptions;
using GrafanaAlerts.Models;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Services
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
            var description = request.Message;
            ThrowIfNull(description, nameof(request.Message), request);
            
            var tags = request.Tags;
            ThrowIfNull(tags, nameof(request.Tags), request);
            
            var role = tags[RoleTag];
            var ke = tags[KeTag];
            var priority = tags[PriorityTag];
            
            ThrowIfNull(role, nameof(RoleTag), request);
            ThrowIfNull(ke, nameof(KeTag), request);
            ThrowIfNull(priority, nameof(PriorityTag), request);

            var ticket = new TroubleTicket
            {
                Description = description,
                Ke = ke,
                Role = role,
                Priority = priority
            };

            return ticket;
        }

        private void ThrowIfNull(object nullable, string name, TriggerAlertRequest request)
        {
            if (nullable != null) return;
            
            _logger.LogError("Found empty property {Name} while processing request {@Request}", name, request);
            throw new GrafanaAlertParseException($"Empty property {name}", request);
        }
    }
}