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
            TroubleTicketDTO ticket;
            
            // Check if there is information about this ticket in database
            if (TicketExists(ticketId) == false)
            {
                _logger.LogError(
                    "There is no such a ticket in the database!");
                
                return HttpStatusCode.BadRequest;
            }

            // Get ticket DTO from database
            ticket = Get(ticketId);
            
            // Send OK message to ticket system
            var result = await _remedy.Close(ticket);

            // Update ticket closed date
            await Update(ticket);

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

            var affected = await connection.ExecuteAsync(query, ticketDto);
            
            _logger.LogInformation("Ticket added. {RowsAffected} rows affected", affected);
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

        private async Task Update(TroubleTicketDTO ticket)
        {
            using var connection = OpenConnection(_connectionString);

            const string query = "update Troubles set ClosedDate = @ClosedDate where TroubleId = @TroubleId";

            var affected = await connection.ExecuteAsync(query, ticket);
            
            _logger.LogInformation("Ticket updated, {RowsAffected} rows affected", affected);
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