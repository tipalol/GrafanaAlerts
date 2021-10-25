using GrafanaAlerts.Core.Builders;

namespace GrafanaAlerts.Infrastructure.Builders
{
    public class RequestBuilder : IRequestBuilder
    {
        private string _request;

        public RequestBuilder(string request)
        {
            _request = request;
        }

        public IRequestBuilder SetAttribute(string name, string value)
        {
            _request = _request.Replace($"[{name}]", value);

            return this;
        }

        public string Build() => _request;
    }
}