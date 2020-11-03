using System;
using System.Collections.Generic;
using System.Text;

namespace Aijkl.CloudFlare.Cache.Models
{
    public class AppSettings
    {
        public CloudFlare CloudFlare { set; get; }
        public GitHub GitHub { set; get; }
        public Core Core { set; get; }
    }
    public class Core
    {
        public int Interval { set; get; }
        public string Url { set; get; }
    }
    public class GitHub
    {
        public string Token { set; get; }
        public string Branch { set; get; }
        public string Repository { set; get; }
        public string Passworld { set; get; }
        public string Username { set; get; }
        public string UserAgent { set; get; }
    }
    public class CloudFlare
    {
        public string Zone { set; get; }
        public string EmailAdress { set; get; }
        public string AuthToken { set; get; }
    }
}
