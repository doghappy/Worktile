using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Web.Http.Filters;

namespace Worktile.Common.WtRequestClient
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

        public static async Task<T> GetAsync<T>(string url)
        {
            var resMsg = await _client.GetAsync(GetUri(url));
            if (resMsg.IsSuccessStatusCode)
            {
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                return default(T);
            }
        }

        public static async Task<JToken> GetJTokenAsync(string url)
        {
            var resMsg = await _client.GetAsync(GetUri(url));
            if (resMsg.IsSuccessStatusCode)
            {
                string json = await resMsg.Content.ReadAsStringAsync();
                using (var reader = new StringReader(json))
                {
                    return await JToken.ReadFromAsync(new JsonTextReader(reader));
                }
            }
            else
            {
                return default(JToken);
            }
        }

        public static async Task<T> PostAsync<T>(string url)
        {
            var resMsg = await _client.PostAsync(GetUri(url), null);
            if (resMsg.IsSuccessStatusCode)
            {
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                return default(T);
            }
        }

        public static async Task<T> PostAsync<T>(string url, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return await PostAsync<T>(url, json);
        }

        public static async Task<JObject> RawPostAsync(string url, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return await RawPostAsync(url, json);
        }

        public static async Task<T> PostAsync<T>(string url, string json)
        {
            var content = new HttpStringContent(json, UnicodeEncoding.Utf8, ApplicationJson);
            return await PostAsync<T>(url, content);
        }

        public static async Task<JObject> RawPostAsync(string url, string json)
        {
            var content = new HttpStringContent(json, UnicodeEncoding.Utf8, ApplicationJson);
            return await RawPostAsync(url, content);
        }

        public static async Task<T> PostAsync<T>(string url, IHttpContent content)
        {
            var resMsg = await _client.PostAsync(GetUri(url), content);
            if (resMsg.IsSuccessStatusCode)
            {
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                return default(T);
            }
        }

        public static async Task<JObject> RawPostAsync(string url, IHttpContent content)
        {
            var resMsg = await _client.PostAsync(GetUri(url), content);
            if (resMsg.IsSuccessStatusCode)
            {
                var json = await resMsg.Content.ReadAsStringAsync();
                return JObject.Parse(json);
            }
            else
            {
                return default(JObject);
            }
        }

        public static async Task<T> PutAsync<T>(string url)
        {
            return await PutAsync<T>(url, null);
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
                return default(T);
            }
        }

        public static async Task<T> PutAsync<T>(string url, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new HttpStringContent(json, UnicodeEncoding.Utf8, ApplicationJson);
            return await PutAsync<T>(url, content);
        }

        public static async Task<T> DeleteAsync<T>(string url)
        {
            var resMsg = await _client.DeleteAsync(GetUri(url));
            if (resMsg.IsSuccessStatusCode)
            {
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                return default(T);
            }
        }
    }
}
