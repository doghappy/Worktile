using Worktile.WindowsUI.Models.Start;

namespace Worktile.WindowsUI.Common
{
    public static class Configuration
    {
        static Configuration()
        {
            BaseAddress = "https://worktile.com";
        }

        public const string UserVaultResource = "User";
        public const string DomainVaultResource = "Domain";

        public static string BaseAddress { get; set; }
        public static TeamConfig TeamConfig { get; set; }
        public static TeamLite TeamLite { get; set; }
        public static bool IsAuthorized { get; set; }
    }
}
