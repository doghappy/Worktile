using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.WindowsUI.Models.Start;

namespace Worktile.WindowsUI.Common
{
    public static class Configuration
    {
        static Configuration()
        {
            BaseAddress = "https://worktile.com";
        }

        public static string BaseAddress { get; set; }

        public static Config TeamConfig { get; set; }
        public static TeamLite TeamLite { get; set; }
    }
}
