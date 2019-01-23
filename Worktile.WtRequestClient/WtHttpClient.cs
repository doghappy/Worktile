using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Worktile.WtRequestClient
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

        public static void SetBaseAddress(string uri)
        {
            if (HttpClient != null)
            {
                HttpClient.Dispose();
                HttpClient = null;
            }
            HttpClient.BaseAddress = new Uri(uri);
        }

        public static Cookie GetCookieByString(string value)
        {
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

        public async Task<byte[]> GetByteArrayAsync(string uri)
        {
            return await HttpClient.GetByteArrayAsync(uri);
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var resMsg = await HttpClient.GetAsync(uri);
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

        public async Task<JToken> GetJTokenAsync(string uri)
        {
            var resMsg = await HttpClient.GetAsync(uri);
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

        public async Task<T> PostAsync<T>(string uri, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            var resMsg = await HttpClient.PostAsync(uri, content);
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

        public async Task<T> PutAsync<T>(string uri)
        {
            var resMsg = await HttpClient.PutAsync(uri, null);
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

        public async Task<T> PutAsync<T>(string uri, object data)
        {
            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            var resMsg = await HttpClient.PutAsync(uri, content);
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
