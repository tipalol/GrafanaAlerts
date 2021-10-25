namespace GrafanaAlerts.Core.Builders
{
    public interface IRequestBuilder
    {
        public IRequestBuilder SetAttribute(string name, string value);

        public string Build();
    }
}