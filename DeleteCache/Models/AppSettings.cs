using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aijkl.CloudFlare.Cache.Models
{
    public class AppSettings
    {
        [JsonProperty("cloudflare")]
        public CloudFlare CloudFlare { set; get; }

        [JsonProperty("github")]
        public GitHub GitHub { set; get; }

        [JsonProperty("core")]
        public Core Core { set; get; }
    }
    public class Core
    {
        [JsonProperty("baseUrl")]
        public string BAseUrl { set; get; }
    }
    
    public class GitHub
    {
        [JsonProperty("token")]
        public string Token { set; get; }

        [JsonProperty("branch")]
        public string Branch { set; get; }

        [JsonProperty("repository")]
        public string Repository { set; get; }

        [JsonProperty("passWord")]
        public string Passworld { set; get; }

        [JsonProperty("userName")]
        public string Username { set; get; }

        [JsonProperty("userAgent")]
        public string UserAgent { set; get; }
    }
    public class CloudFlare
    {
        [JsonProperty("zone")]
        public string Zone { set; get; }

        [JsonProperty("emailAdress")]
        public string EmailAdress { set; get; }

        [JsonProperty("authToken")]
        public string AuthToken { set; get; }
    }
}
