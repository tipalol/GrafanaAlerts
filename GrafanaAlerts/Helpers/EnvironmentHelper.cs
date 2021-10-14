using System;
using System.Threading;

namespace GrafanaAlerts.Helpers
{
    public static class EnvironmentHelper
    {
        private const string TicketsEnvironmentName = "GRAFANA_TICKETS";
        private static int _tickets = -1;

        public static int GetTicketsCount()
        {
            if (_tickets == -1)
            {
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
            }
            
            return _tickets;
        }

        public static int AddTicket()
        {
            Interlocked.Increment(ref _tickets);

            return _tickets;
        }
    }
}