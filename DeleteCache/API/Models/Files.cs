using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aijkl.CloudFlare.Cache.API.Models
{
    class PurgeRequestObject
    {
        [JsonProperty("files")]
        public List<string> Files { set; get; }
    }
}
