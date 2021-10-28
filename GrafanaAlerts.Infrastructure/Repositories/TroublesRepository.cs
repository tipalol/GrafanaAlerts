using System;
using System.Data;
using System.Linq;
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
            if (TicketExists(ticket))
            {
                _logger.LogInformation("This ticket has been already registered and it is not closed!");
                return HttpStatusCode.BadRequest;
            }

            // Registering ticket in trouble system
            var troubleId = await _remedy.Accept(ticket);

            // Record ticket data in database
            await RecordTicket(ticket, troubleId);

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> Close(int ticketId)
        {
            // Check if the ticket has been already registered
            if (TicketExists(ticketId) == false)
            {
                _logger.LogInformation(
                    "There is no such a ticket in the database! Anyway, trying to close ticket..");
                // return HttpStatusCode.BadRequest;
            }

            var ticket = Get(ticketId);
            var result = await _remedy.Close(ticket);

            return HttpStatusCode.OK;
        }

        private async Task RecordTicket(TroubleTicket ticket, string troubleId)
        {
            var ticketDto = new TroubleTicketDTO()
            {
                AlertId = ticket.Id,
                TroubleId = troubleId,
                CreationDate = DateTime.Now,
                ClosedDate = DateTime.MinValue
            };

            using var connection = OpenConnection(_connectionString);

            const string query = "insert into Troubles (AlertId, TroubleId, CreationDate, ClosedDate) values (@AlertId, @TroubleId, @CreationDate, @ClosedDate)";

            await connection.ExecuteAsync(query, ticketDto);
        }

        private bool TicketExists(TroubleTicket ticket)
        {
            return TicketExists(ticket.Id);
        }

        private bool TicketExists(int ticketId)
        {
            using var connection = OpenConnection(_connectionString);

            var recorded = connection.Query<TroubleTicketDTO>("select * from Troubles where AlertId = @Id", ticketId);

            var troubleTickets = recorded as TroubleTicketDTO[] ?? recorded.ToArray();
            
            if (troubleTickets.Length == 0)
                return false;
            
            return troubleTickets[0].ClosedDate != DateTime.MinValue;
        }
        
        private TroubleTicketDTO Get(int ticketId)
        {
            using var connection = OpenConnection(_connectionString);

            var recorded = connection.Query<TroubleTicketDTO>("select * from Troubles where AlertId = @Id", ticketId);

            var troubleTickets = recorded as TroubleTicketDTO[] ?? recorded.ToArray();

            if (troubleTickets.Length == 0)
                throw new ArgumentException("There is no ticket with specified name", nameof(ticketId));

            return troubleTickets[0];
        }

        private static IDbConnection OpenConnection(string connectionString)
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            return connection;
        }
    }
}