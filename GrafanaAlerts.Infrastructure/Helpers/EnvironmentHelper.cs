using System;
using System.Threading;

namespace GrafanaAlerts.Infrastructure.Helpers
{
    public static class EnvironmentHelper
    {
        private const string TicketsEnvironmentName = "GRAFANA_TICKETS";
        private static int _tickets = -1;
        
        public static int GetTicketsCount()
        {
            // If there is cached value
            if (_tickets != -1) return _tickets;
            
            // if not - try to load one from environment
            var tickets = Environment.GetEnvironmentVariable(TicketsEnvironmentName);
            
            if (tickets != null)
            {
                _tickets = Convert.ToInt32(tickets);
            }
            else
            {
                _tickets = 0;
                Environment.SetEnvironmentVariable(TicketsEnvironmentName, $"{_tickets}");
                    
            }

            return _tickets;
        }

        /// <summary>
        /// Increments count of registered tickets
        /// </summary>
        /// <returns></returns>
        public static int AddTicket()
        {
            if (_tickets == -1)
                GetTicketsCount();
            
            Interlocked.Increment(ref _tickets);

            return _tickets;
        }
    }
}