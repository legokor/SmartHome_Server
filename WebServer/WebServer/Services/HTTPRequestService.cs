using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebServer.Services
{
    public class HTTPRequestService
    {
        private HttpClient httpClient;

        public HTTPRequestService()
        {
            httpClient = new HttpClient();
        }

        public String getRequest(String ip)
        {
            var result = httpClient.GetAsync("https://www.google.com").Result;
            return result.StatusCode.ToString();
        }
    }
}
