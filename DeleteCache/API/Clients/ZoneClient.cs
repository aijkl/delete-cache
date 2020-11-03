using Aijkl.CloudFlare.API;
using Aijkl.CloudFlare.Cache.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aijkl.CloudFlare.Cache.API.Clients
{
    class ZoneClient
    {
        private readonly APIClient apiClient;
        public ZoneClient(APIClient apiClient)
        {
            this.apiClient = apiClient;
        }        
        public async Task<APIResult<Response>> PurgeFilesByUrl(string zone,List<string> urls)
        {
            var request = Request.CreatePostRequest($"zones/{zone}/purge_cache", json: JsonConvert.SerializeObject(new PurgeRequestObject()
            {
                Files = urls
            }));
            return await apiClient.SendRequestAsync<APIResult<Response>>(request);
        }
    }
}
