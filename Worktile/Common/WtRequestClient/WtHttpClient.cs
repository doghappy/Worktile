using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Worktile.Common.WtRequestClient
{
    public class WtHttpClient
    {
        static HttpClient _httpClient;
        static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }
                return _httpClient;
            }
            set
            {
                _httpClient = value;
            }
        }


        public const string ApplicationJson = "application/json";

        public event Action<HttpResponseMessage> OnSuccessStatusCode;
        public event Action<HttpResponseMessage> OnNonSuccessStatusCode;

        public bool IsSuccessStatusCode { get; private set; }

        public static void AddDefaultRequestHeaders(string name, string value)
        {
            HttpClient.DefaultRequestHeaders.Add(name, value);
        }

        public static void SetBaseAddress(string url)
        {
            if (HttpClient != null)
            {
                HttpClient.Dispose();
                HttpClient = null;
            }
            HttpClient.BaseAddress = new Uri(url);
        }

        public static Cookie GetCookieByString(string value)
        {
            //var container = new CookieContainer();
            //container.Add()
            string[] arr = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] name = arr[0].Split('=');
            string[] domain = arr[1].Split('=');
            string[] path = arr[2].Split('=');
            string[] expires = arr[3].Split('=');
            return new Cookie
            {
                Name = name.First(),
                Value = name.Last(),
                Domain = domain.Last(),
                Path = path.Last(),
                Expires = DateTime.Parse(expires.Last())
            };
        }

        public static string GetStringByCookie(Cookie cookie)
        {
            const string separator = "; ";
            return cookie.Name + "=" + cookie.Value + separator
                + "Domain=" + cookie.Domain + separator
                + "Path=" + cookie.Path + separator
                + "Expires=" + cookie.Expires.ToString("r");
        }

        public static void Reset() => _httpClient = null;

        public async Task<byte[]> GetByteArrayAsync(string url)
        {
            return await HttpClient.GetByteArrayAsync(url);
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var resMsg = await HttpClient.GetAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                IsSuccessStatusCode = true;
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                OnNonSuccessStatusCode?.Invoke(resMsg);
                return default(T);
            }
        }

        public async Task<JToken> GetJTokenAsync(string url)
        {
            var resMsg = await HttpClient.GetAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                IsSuccessStatusCode = true;
                string json = await resMsg.Content.ReadAsStringAsync();
                using (var reader = new StringReader(json))
                {
                    return await JToken.ReadFromAsync(new JsonTextReader(reader));
                }
            }
            else
            {
                OnNonSuccessStatusCode?.Invoke(resMsg);
                return default(JToken);
            }
        }

        public async Task<T> PostAsync<T>(string url)
        {
            var resMsg = await HttpClient.PostAsync(url, null);
            if (resMsg.IsSuccessStatusCode)
            {
                IsSuccessStatusCode = true;
                OnSuccessStatusCode?.Invoke(resMsg);
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                OnNonSuccessStatusCode?.Invoke(resMsg);
                return default(T);
            }
        }

        public async Task<T> PostAsync<T>(string url, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            return await PostAsync<T>(url, json);
        }

        public async Task<T> PostAsync<T>(string url, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            return await PostAsync<T>(url, content);
        }

        public async Task<T> PostAsync<T>(string url, HttpContent content)
        {
            var resMsg = await HttpClient.PostAsync(url, content);
            if (resMsg.IsSuccessStatusCode)
            {
                IsSuccessStatusCode = true;
                OnSuccessStatusCode?.Invoke(resMsg);
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                OnNonSuccessStatusCode?.Invoke(resMsg);
                return default(T);
            }
        }

        public async Task<T> PutAsync<T>(string url)
        {
            return await PutAsync<T>(url, null);
        }

        public async Task<T> PutAsync<T>(string url, HttpContent content)
        {
            var resMsg = await HttpClient.PutAsync(url, content);
            if (resMsg.IsSuccessStatusCode)
            {
                IsSuccessStatusCode = true;
                OnSuccessStatusCode?.Invoke(resMsg);
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                OnNonSuccessStatusCode?.Invoke(resMsg);
                return default(T);
            }
        }

        public async Task<T> PutAsync<T>(string url, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            return await PutAsync<T>(url, content);
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            var resMsg = await HttpClient.DeleteAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                IsSuccessStatusCode = true;
                OnSuccessStatusCode?.Invoke(resMsg);
                return await resMsg.ReadAsModelAsync<T>();
            }
            else
            {
                OnNonSuccessStatusCode?.Invoke(resMsg);
                return default(T);
            }
        }
    }
}
