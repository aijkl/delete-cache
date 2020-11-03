using Aijkl.CloudFlare.API;
using Aijkl.CloudFlare.Cache.API.Clients;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Aijkl.CloudFlare.Cache
{
    class CloudFlareClient : IDisposable
    {        
        private APIClient apiClient;
        public CloudFlareClient(string emailAdress, string authKey)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.All,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            apiClient = new APIClient(new HttpClient(httpClientHandler));
            apiClient.AddHeader("X-Auth-Email", emailAdress);
            apiClient.AddHeader("X-Auth-Key", authKey);
        }
        public ZoneClient Zone => new ZoneClient(apiClient);
        public void Dispose()
        {
            apiClient?.Dispose();
            apiClient = null;
        }
    }
}
