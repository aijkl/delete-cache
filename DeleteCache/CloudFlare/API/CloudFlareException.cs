using Aijkl.CloudFlare.Cache.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Aijkl.CloudFlare.API
{
    public class CloudFlareException : Exception
    {
        public CloudFlareException(HttpResponseMessage httpResponse) : base(httpResponse.StatusCode.ToString())
        {
            Result = new APIResult<Response>()
            {
                Etag = httpResponse.Headers.ETag,
                ResponseBody = httpResponse.Content.ReadAsStringAsync().Result,
                HttpResponseHeaders = httpResponse.Headers,
                Result = JsonConvert.DeserializeObject<Response>(httpResponse.Content.ReadAsStringAsync().Result)
            };
        }
        public APIResult<Response> Result;        
    }
}
