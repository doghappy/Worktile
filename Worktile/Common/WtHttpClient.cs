using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Web.Http.Filters;

namespace Worktile.Common
{
    public static class WtHttpClient
    {
        static WtHttpClient()
        {
            _client = new HttpClient(new HttpBaseProtocolFilter());
        }

        readonly static HttpClient _client;

        public static HttpClient Client => _client;

        const string ApplicationJson = "application/json";

        public static string Domain { get; set; }

        private static Uri GetUri(string url)
        {
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            if (url.StartsWith("http"))
            {
                return new Uri(url);
            }
            else
            {
                return new Uri($"https://{Domain}.worktile.com/{url}");
            }
        }

        public static async Task<IBuffer> GetByteBufferAsync(string url)
        {
            return await _client.GetBufferAsync(GetUri(url));
        }

        public static async Task<JObject> GetAsync(string url)
        {
            var json = await _client.GetStringAsync(GetUri(url));
            return JObject.Parse(json);
        }

        public static async Task<JObject> PostAsync(string url, string json)
        {
            var content = new HttpStringContent(json, UnicodeEncoding.Utf8, ApplicationJson);
            return await PostAsync(url, content);
        }

        public static async Task<JObject> PostAsync(string url)
        {
            return await PostAsync(url, default(IHttpContent));
        }

        public static async Task<JObject> PostAsync(string url, IHttpContent content)
        {
            var resMsg = await _client.PostAsync(GetUri(url), content);
            if (resMsg.IsSuccessStatusCode)
            {
                string json= await resMsg.Content.ReadAsStringAsync();
                return JObject.Parse(json);
            }
            else
            {
                return default;
            }
        }

        public static async Task<JObject> DeleteAsync(string url)
        {
            var resMsg = await _client.DeleteAsync(GetUri(url));
            if (resMsg.IsSuccessStatusCode)
            {
                string json = await resMsg.Content.ReadAsStringAsync();
                return JObject.Parse(json);
            }
            else
            {
                return default;
            }
        }

        public static async Task<T> PutAsync<T>(string url, IHttpContent content)
        {
            var resMsg = await _client.PutAsync(GetUri(url), content);
            if (resMsg.IsSuccessStatusCode)
            {
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                return default;
            }
        }
    }
}
