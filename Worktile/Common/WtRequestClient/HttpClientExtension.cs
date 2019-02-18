using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

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
