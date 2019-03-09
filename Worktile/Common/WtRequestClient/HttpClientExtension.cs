using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Web.Http;
using System;

namespace Worktile.Common.WtRequestClient
{
    public static class HttpClientExtension
    {
        public async static Task<T> ReadAsModelAsync<T>(this HttpResponseMessage resMsg)
        {
            string json = await resMsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
