using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Aijkl.CloudFlare.API
{
    public class APIResult<T>
    {        
        public T Result { set; get; }
        public EntityTagHeaderValue Etag { set; get; }
        public string ResponseBody { set; get; }
        public HttpResponseHeaders HttpResponseHeaders { set; get; }
        public HttpStatusCode HttpStatusCode { set; get; }
    }
}
