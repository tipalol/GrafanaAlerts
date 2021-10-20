using System.IO;
using GrafanaAlerts.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrafanaAlerts.Services.Implementations
{
    internal sealed class RequestProviderService : IRequestProviderService
    {
        private readonly string _requestsLocationPath;
        private readonly ILogger<RequestProviderService> _logger;

        public RequestProviderService(ILogger<RequestProviderService> logger)
        {
            _requestsLocationPath =  ConfigHelper.Load().App.SoapRequestsLocation;
            _logger = logger;
        }
        
        public string GetRequest(string requestName)
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