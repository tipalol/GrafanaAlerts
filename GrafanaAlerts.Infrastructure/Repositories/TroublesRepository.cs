using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using GrafanaAlerts.Core.Entities;
using GrafanaAlerts.Core.Repositories;
using GrafanaAlerts.Infrastructure.Configuration;
using GrafanaAlerts.Infrastructure.DTO;
using GrafanaAlerts.Infrastructure.Integrations;
using Microsoft.Extensions.Logging;
using Dapper;
using Npgsql;

namespace GrafanaAlerts.Infrastructure.Repositories
{
    public class TroublesRepository : ITroublesRepository
    {
        private readonly ILogger<TroublesRepository> _logger;
        private readonly IRemedyVisitor _remedy;

        private readonly string _connectionString;

        public TroublesRepository(ILogger<TroublesRepository> logger, IRemedyVisitor remedy)
        {
            _connectionString = ConfigProvider.Load().App.DatabaseConnectionString;
            _logger = logger;
            _remedy = remedy;
        }

        public async Task<HttpStatusCode> Add(TroubleTicket ticket)
        {
            // Check if the ticket has been already registered
            /*
            var ticketExists = await TicketExists(ticket);
            if (ticketExists)
                return HttpStatusCode.BadRequest;*/
            
            // Registering ticket in trouble system
            var troubleId = await _remedy.AcceptTroubleTicket(ticket);

            // Record ticket data in database
            // await RecordTicket(ticket, troubleId);

            return HttpStatusCode.OK;
        }
        
        public void Dispose()
        {
            
        }
        
        private async Task RecordTicket(TroubleTicket ticket, string troubleId)
        {
            var ticketDto = new TroubleTicketDTO()
            {
                AlertId = ticket.Id,
                TroubleId = troubleId,
                CreationDate = DateTime.Now
            };

            using var connection = OpenConnection(_connectionString);

            const string query = "insert into Troubles (AlertId, TroubleId, CreationDate, ClosedDate) values (@AlertId, @TroubleId, @CreationDate, @ClosedDate)";

            await connection.ExecuteAsync(query, ticketDto);
        }

        private async Task<bool> TicketExists(TroubleTicket ticket)
        {
            using var connection = OpenConnection(_connectionString);

            const string query = "select * from Troubles where ";
            
            return true;
        }

        private static IDbConnection OpenConnection(string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            return connection;
        }
    }
}