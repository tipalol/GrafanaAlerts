using System;

namespace GrafanaAlerts.Infrastructure.DTO
{
    public class TroubleTicketDTO
    {
        public int AlertId { get; set; }
        public string TroubleId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}