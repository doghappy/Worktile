using Newtonsoft.Json.Linq;

namespace Worktile.Infrastructure
{
    public static class JTokenHelper
    {
        public static T GetPropertyValue<T>(JToken obj, string key)
        {
            string[] keys = key.Split('.');

            foreach (var k in keys)
            {
                obj = obj[k];
            }
            if (obj != null)
            {
                return obj.ToObject<T>();
            }
            return default(T);
        }
    }
}
