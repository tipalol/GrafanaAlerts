using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GrafanaAlerts.Models;
using Microsoft.Extensions.Configuration;

namespace GrafanaAlerts.Services
{
    internal sealed class TicketRegisterService : ITicketRegisterService, IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _troubleTicketSystemHost;
        
        
        public TicketRegisterService(IConfiguration configuration)
        {
            _client = new HttpClient();
            _troubleTicketSystemHost = configuration["TroubleTicketSystemHost"];
        }
        
        public Task<HttpStatusCode> Register(TroubleTicket ticket)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}