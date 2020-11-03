﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Aijkl.CloudFlare.API
{
    internal static class Request
    {
        private const string BASE_URL = "https://api.cloudflare.com/client/v4";        
        internal static HttpRequestMessage CreateGetRequest(string verb, string etag = "")
        {
            HttpRequestMessage httpRequestMessage = CreateDefaultRequest(etag);
            httpRequestMessage.Method = HttpMethod.Get;
            string conjunction = Regex.Match(verb, @"(.+)=\d+").Success ? "&" : "?";
            httpRequestMessage.RequestUri = new Uri($"{BASE_URL}/{verb}");
            return httpRequestMessage;
        }
        internal static HttpRequestMessage CreatePutRequest(string verb, string eTag = "", string json = "")
        {
            HttpRequestMessage httpRequestMessage = CreateDefaultRequest(eTag);
            httpRequestMessage.Method = HttpMethod.Put;
            string conjunction = Regex.Match(verb, @"(.+)=\d+").Success ? "&" : "?";
            httpRequestMessage.RequestUri = new Uri($"{BASE_URL}/{verb}");
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, @"application/json");
            return httpRequestMessage;
        }
        internal static HttpRequestMessage CreatePostRequest(string verb, string eTag = "", string json = "")
        {
            HttpRequestMessage httpRequestMessage = CreateDefaultRequest(eTag);
            httpRequestMessage.Method = HttpMethod.Post;
            string conjunction = Regex.Match(verb, @"(.+)=\d+").Success ? "&" : "?";
            httpRequestMessage.RequestUri = new Uri($"{BASE_URL}/{verb}");
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, @"application/json");
            return httpRequestMessage;
        }
        internal static HttpRequestMessage CreateDeleteRequest(string verb, string eTag = "")
        {
            HttpRequestMessage httpRequestMessage = CreateDefaultRequest(eTag);
            httpRequestMessage.Method = HttpMethod.Delete;
            string conjunction = Regex.Match(verb, @"(.+)=\d+").Success ? "&" : "?";
            httpRequestMessage.RequestUri = new Uri($"{BASE_URL}/{verb}");
            return httpRequestMessage;
        }
        private static HttpRequestMessage CreateDefaultRequest(string etag)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();            
            httpRequestMessage.Headers.Add("Connection", "close");
            httpRequestMessage.Headers.Add("Cache-Control", "max-age=0");            
            httpRequestMessage.Headers.Add("Accept", "*/*");
            httpRequestMessage.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            httpRequestMessage.Headers.Add("Accept-Encoding", "gzip");
            if (etag != string.Empty) httpRequestMessage.Headers.Add("If-None-Match", etag);

            return httpRequestMessage;
        }
    }
}
