using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worktile.WindowsUI.Common
{
    public static class Configuration
    {
        static Configuration()
        {
            BaseAddress = "https://worktile.com";
        }

        public static string BaseAddress { get; set; }
    }
}
