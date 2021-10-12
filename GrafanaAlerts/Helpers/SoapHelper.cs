using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace GrafanaAlerts.Helpers
{
    internal static class SoapHelper
    {
        public static async Task<string> SendRequest(string url, string soap)
        {
            var request = CreateWebRequest(url);
            var xml = new XmlDocument();
            
            xml.LoadXml(soap);

            await using var stream = request.GetRequestStream();
            xml.Save(stream);

            using var response = request.GetResponse();
            using var reader = new StreamReader(response.GetResponseStream() ?? 
                                                throw new InvalidOperationException("Response stream was null!"));

            var result = await reader.ReadToEndAsync();

            return result;
        }
        
        private static HttpWebRequest CreateWebRequest(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            
            return webRequest;
        }
    }
}