namespace GrafanaAlerts.Services
{
    public interface IRequestProviderService
    {
        public string GetRequest(string requestName);
    }
}