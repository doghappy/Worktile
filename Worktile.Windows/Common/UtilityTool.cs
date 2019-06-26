using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Worktile.Windows.Common
{
    public static class UtilityTool
    {
        public static string GetStringFromResources(string key)
        {
            var srcLoader = ResourceLoader.GetForCurrentView();
            return srcLoader.GetString(key);
        }
    }
}
