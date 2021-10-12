using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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

        private string RegisterTicketApiMethod => $"{_troubleTicketSystemHost}/createTT";
        
        public async Task<HttpStatusCode> Register(TroubleTicket ticket)
        {
            var response = await _client.PostAsJsonAsync(RegisterTicketApiMethod, ticket);

            return response.StatusCode;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}