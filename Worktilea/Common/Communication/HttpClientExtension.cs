using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Web.Http;
using System;

namespace Worktile.Common.Communication
{
    public static class HttpClientExtension
    {
        //static HttpClientExtension()
        //{
        //    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        //    {
        //        DateTimeZoneHandling = DateTimeZoneHandling.Local,
        //    };
        //}

        public async static Task<T> ReadAsModelAsync<T>(this HttpResponseMessage resMsg)
        {
            string json = await resMsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
