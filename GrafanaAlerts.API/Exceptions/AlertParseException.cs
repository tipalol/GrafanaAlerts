using System;
using GrafanaAlerts.API.Models;

namespace GrafanaAlerts.API.Exceptions
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