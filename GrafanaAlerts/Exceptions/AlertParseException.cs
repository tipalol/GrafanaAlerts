using System;
using GrafanaAlerts.Models;

namespace GrafanaAlerts.Exceptions
{
    public sealed class GrafanaAlertParseException : Exception
    {
        public TriggerAlertRequest Request { get; }

        public GrafanaAlertParseException(string message, TriggerAlertRequest request) : base(message)
        {
            Request = request;
        }
    }
}