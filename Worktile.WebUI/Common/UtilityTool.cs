using Windows.ApplicationModel.Resources;

namespace Worktile.WebUI.Common
{
    public static class UtilityTool
    {
        public static string GetStringFromResources(string key)
        {
            var srcLoader = ResourceLoader.GetForViewIndependentUse();
            return srcLoader.GetString(key);
        }
    }
}
