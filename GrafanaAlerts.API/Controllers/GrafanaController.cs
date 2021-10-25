using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GrafanaAlerts.API.Exceptions;
using GrafanaAlerts.API.Models;
using GrafanaAlerts.API.Services;
using GrafanaAlerts.Core.Entities;
using GrafanaAlerts.Core.Repositories;
using GrafanaAlerts.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;

namespace GrafanaAlerts.API.Controllers
{
    [ApiController]
    [Route("api")]
    public sealed class GrafanaController : ControllerBase
    {
        private const string GrafanaAlertParseError = "Error while parsing Grafana alert. Maybe there are some missing tags";
        private const string RegisteringTicketError = "Ticket management syste, returned error status code.";
        private const string ServiceIsNotAvailableError = "Server is not available now.";
        private const string HttpRequestError = "Error while sending request to the ticket system.";
        private const string ConnectingTicketSystemError = "Error while connecting the ticket system. Maybe there is wrong url.";

        private static readonly ActivitySource ActivitySource = new (nameof(GrafanaController));
        
        private readonly ILogger<GrafanaController> _logger;
        private readonly IGrafanaAlertParseService _parser;
        private readonly IAdditionalTroubleInformationProviderService _complementor;
        private readonly ITroublesRepository _ticketRepository;
        
        public GrafanaController(
            ILogger<GrafanaController> logger, 
            IGrafanaAlertParseService parser,
            IAdditionalTroubleInformationProviderService complementor,
            ITroublesRepository ticketRepository)
        {
            _logger = logger;
            _parser = parser;
            _complementor = complementor;
            _ticketRepository = ticketRepository;
        }
        
        /// <summary>
        /// Action called by Grafana when alert was triggered
        /// </summary>
        /// <param name="request">Grafana request body</param>
        [HttpPost("trigger")]
        public async Task<HttpStatusCode> TriggerAlert([FromBody] TriggerAlertRequest request)
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
                    _logger.LogError(GrafanaAlertParseError + "Exception: {@Exception}", exception);
                    
                    StopWithError(activity, GrafanaAlertParseError, exception);

                    return HttpStatusCode.BadRequest;
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
                    _logger.LogError(ServiceIsNotAvailableError + 
                                     "Service {Service} is not available. Exception: {@Exception}",
                            exception.ServiceName, exception);
                    
                    StopWithError(activity, ServiceIsNotAvailableError, exception);

                    return HttpStatusCode.ServiceUnavailable;
                }
            }

            activityName = $"{nameof(ITroublesRepository)} is registering ticket";
            using (var activity = ActivitySource.StartActivity(activityName, ActivityKind.Producer))
            {
                _logger.LogInformation("Ticket complemented to {@CompleteTicket}. Registering..", completeTicket);
                try
                {
                    var result = await _ticketRepository.Add(completeTicket);
                    
                    _logger.LogInformation("Ticket registered. Status: {Result}", result);

                    if (result == HttpStatusCode.OK) return HttpStatusCode.OK;

                    _logger.LogError(RegisteringTicketError + "Status: {Result}", result);
                    
                    StopWithError(activity, RegisteringTicketError, 
                        new Exception(RegisteringTicketError + $"Status: {result}"));
                    
                    return HttpStatusCode.BadRequest;
                }
                catch (ServiceNotRespondingException exception)
                {
                    _logger.LogError(
                        ServiceIsNotAvailableError + " {Service} is not available. Exception: {@Exception}",
                        exception.ServiceName, exception);
                    
                    StopWithError(activity, ServiceIsNotAvailableError, exception);

                    return HttpStatusCode.ServiceUnavailable;
                }
                catch (HttpRequestException exception)
                {
                    _logger.LogError(HttpRequestError + " Exception: {@Exception}", exception);
                    
                    StopWithError(activity, HttpRequestError, exception);
                    
                    return HttpStatusCode.BadRequest;
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogError(ConnectingTicketSystemError + " Exception: {@Exception}", exception);
                    
                    StopWithError(activity, ConnectingTicketSystemError, exception);

                    return HttpStatusCode.BadRequest;
                }
            }
        }

        [HttpPost("troubles")]
        public async Task<HttpStatusCode> CreateTrouble(TroubleTicket ticket)
        {
            _logger.LogInformation("Got new custom trouble request: {@Ticket}. Registering..", ticket);
            
            var activityName = $"{nameof(ITroublesRepository)} is registering ticket";
            using var activity = ActivitySource.StartActivity(activityName, ActivityKind.Producer);
            
            try
            {
                var result = await _ticketRepository.Add(ticket);
                    
                _logger.LogInformation("Ticket registered. Status: {Result}", result);

                if (result == HttpStatusCode.OK) return HttpStatusCode.OK;

                _logger.LogError(RegisteringTicketError + "Status: {Result}", result);
                    
                StopWithError(activity, RegisteringTicketError, 
                    new Exception(RegisteringTicketError + $"Status: {result}"));
                    
                return HttpStatusCode.BadRequest;
            }
            catch (ServiceNotRespondingException exception)
            {
                _logger.LogError(
                    ServiceIsNotAvailableError + " {Service} is not available. Exception: {@Exception}",
                    exception.ServiceName, exception);
                    
                StopWithError(activity, ServiceIsNotAvailableError, exception);

                return HttpStatusCode.ServiceUnavailable;
            }
            catch (HttpRequestException exception)
            {
                _logger.LogError(HttpRequestError + " Exception: {@Exception}", exception);
                    
                StopWithError(activity, HttpRequestError, exception);
                    
                return HttpStatusCode.BadRequest;
            }
            catch (InvalidOperationException exception)
            {
                _logger.LogError(ConnectingTicketSystemError + " Exception: {@Exception}", exception);
                    
                StopWithError(activity, ConnectingTicketSystemError, exception);

                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPost("debug")]
        public async Task<HttpStatusCode> LogRequest([FromBody] string request)
        {
            _logger.LogInformation("Got request: {@Request}", request);

            return HttpStatusCode.OK;
        }

        private static void StopWithError(Activity activity, string errorMessage, Exception exception)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity.RecordException(exception);
            
            activity?.Parent?
                .SetStatus(ActivityStatusCode.Error)
                .SetTag("http.status_code", 400)
                .AddTag("error.message", errorMessage);
            
            activity?.Parent?.RecordException(exception);
            activity?.Parent?.Stop();
        }
    }
}