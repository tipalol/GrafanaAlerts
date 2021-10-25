namespace GrafanaAlerts.Core.Repositories
{
    public interface IRequestRepository
    {
        public string Get(string requestName);
    }
}