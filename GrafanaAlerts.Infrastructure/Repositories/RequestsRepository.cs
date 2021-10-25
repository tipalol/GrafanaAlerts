using System.IO;
using GrafanaAlerts.Core.Repositories;
using GrafanaAlerts.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Infrastructure.Repositories
{
    public class RequestsRepository : IRequestRepository
    {
        private readonly string _requestsLocationPath;
        private readonly ILogger<RequestsRepository> _logger;

        public RequestsRepository(ILogger<RequestsRepository> logger)
        {
            _requestsLocationPath =  ConfigProvider.Load().App.SoapRequestsLocation;
            _logger = logger;
        }
        
        public string Get(string requestName)
        {
            try
            {
                var request = File.ReadAllText($"{_requestsLocationPath}/{requestName}.xml");

                return request;
            }
            catch (DirectoryNotFoundException exception)
            {
                _logger.LogError("Specific directory not found! Exception: {@Exception}", exception);
                throw;
            }
            catch (FileNotFoundException exception)
            {
                _logger.LogError("Specific file not found! Exception: {@Exception}", exception);
                throw;
            }
        }
    }
}