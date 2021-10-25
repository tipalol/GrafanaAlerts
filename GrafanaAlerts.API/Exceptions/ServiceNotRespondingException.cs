using System;

namespace GrafanaAlerts.API.Exceptions
{
    public sealed class ServiceNotRespondingException : Exception
    {
        public string ServiceName { get; }

        public ServiceNotRespondingException(string message, string serviceName) : base(message)
        {
            ServiceName = serviceName;
        }
    }
}