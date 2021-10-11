using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GrafanaAlerts.Exceptions;
using GrafanaAlerts.Models;
using GrafanaAlerts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace GrafanaAlerts.Controllers
{
    [ApiController]
    [Route("api")]
    public class GrafanaController : ControllerBase
    {
        private static readonly ActivitySource ActivitySource = new (nameof(GrafanaController));
        
        private readonly ILogger<GrafanaController> _logger;
        private readonly IGrafanaAlertParseService _parser;
        private readonly IAdditionalTroubleInformationProviderService _complementor;
        private readonly ITicketRegisterService _ticketRegister;
        
        public GrafanaController(
            ILogger<GrafanaController> logger, 
            IGrafanaAlertParseService parser,
            IAdditionalTroubleInformationProviderService complementor,
            ITicketRegisterService ticketRegister)
        {
            _logger = logger;
            _parser = parser;
            _complementor = complementor;
            _ticketRegister = ticketRegister;
        }
        
        /// <summary>
        /// Action called by Grafana when alert was triggered
        /// </summary>
        /// <param name="request">Grafana request body</param>
        [HttpPost("trigger")]
        public async Task<int?> TriggerAlert([FromBody] TriggerAlertRequest request)
        {
            TroubleTicket ticket;
            TroubleTicket completeTicket;

            var activityName = $"{nameof(IGrafanaAlertParseService)} is parsing alert";
            using (var activity = ActivitySource.StartActivity(activityName, ActivityKind.Consumer))
            {
                _logger.LogInformation("Got new grafana alert. Starting to parse request.. {@Request}", request);
                try
                {
                    ticket = _parser.Parse(request);
                }
                catch (GrafanaAlertParseException exception)
                {
                    _logger.LogError("Error while parsing grafana alert request. Exception: {@Exception}", exception);
                    activity?.SetStatus(ActivityStatusCode.Error);
                    activity.RecordException(exception);
                    
                    return BadRequest(exception).StatusCode;
                }
            }
            
            activityName = $"{nameof(IAdditionalTroubleInformationProviderService)} is complementing ticket";
            using (var activity = ActivitySource.StartActivity(activityName, ActivityKind.Client))
            {
                _logger.LogInformation("Complementing new trouble ticket: {@Ticket}", ticket);
                try
                {
                    completeTicket = await _complementor.Complete(ticket);
                }
                catch (ServiceNotRespondingException exception)
                {
                    _logger.LogError("Error while complementing trouble ticket because service {Service} is not available. Exception: {@Exception}", exception.ServiceName, exception);
                    activity?.SetStatus(ActivityStatusCode.Error);
                    activity.RecordException(exception);
                    
                    return Problem(
                        $"Error while complementing trouble ticket because service {exception.ServiceName} is not available.", 
                        $"{exception}", 
                        503, 
                        "Service Unavailable").StatusCode;
                }
            }

            activityName = $"{nameof(ITicketRegisterService)} is registering ticket";
            using (var activity = ActivitySource.StartActivity(activityName, ActivityKind.Producer))
            {
                _logger.LogInformation("Ticket complemented to {@CompleteTicket}. Registering..", completeTicket);
                try
                {
                    var result = await _ticketRegister.Register(completeTicket);
                    _logger.LogInformation("Ticket registered. Status: {Result}", result);

                    if (result == HttpStatusCode.OK) return Ok(completeTicket).StatusCode;

                    _logger.LogError("Registering status was not OK. Status: {Result}", result);
                    activity?.SetStatus(ActivityStatusCode.Error);
                    
                    return BadRequest($"Status code was not OK but {result}").StatusCode;
                }
                catch (ServiceNotRespondingException exception)
                {
                    _logger.LogError(
                        "Error while registering trouble ticket because service {Service} is not available. Exception: {@Exception}",
                        exception.ServiceName, exception);
                    activity?.SetStatus(ActivityStatusCode.Error);
                    activity.RecordException(exception);
                    
                    return Problem(
                        $"Error while complementing trouble ticket because service {exception.ServiceName} is not available.", 
                        $"{exception}", 
                        503, 
                        "Service Unavailable").StatusCode;
                }
                catch (HttpRequestException exception)
                {
                    _logger.LogError(
                        "Http error while registering trouble ticket. Exception: {@Exception}", exception);
                    activity?.SetStatus(ActivityStatusCode.Error);
                    activity.RecordException(exception);
                    
                    return BadRequest(exception).StatusCode;
                }
            }
        }
    }
}