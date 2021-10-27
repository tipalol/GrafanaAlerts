using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace GrafanaAlerts.Infrastructure.Helpers
{
    public class SoapHelper
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

        public static string GetTroubleId(string ttResponse)
        {
            const string searchTag = "<ns2:internalTTID>";
            var idTagPosition = ttResponse.LastIndexOf(searchTag, StringComparison.Ordinal) + searchTag.Length;
            var id = "";

            for (var i = idTagPosition; i < ttResponse.Length - searchTag.Length - 1; i++)
            {
                id += ttResponse[i];

                if (ttResponse[i] == '<')
                    break;
            }

            return id;
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