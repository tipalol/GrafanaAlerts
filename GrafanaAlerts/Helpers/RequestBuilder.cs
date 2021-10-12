namespace GrafanaAlerts.Helpers
{
    internal sealed class RequestBuilder
    {
        private string _request;

        public RequestBuilder(string request)
        {
            _request = request;
        }

        public RequestBuilder ChangeAttribute(string name, string value)
        {
            _request = _request.Replace($"[{name}]", value);

            return this;
        }

        public string Build() => _request;
    }
}