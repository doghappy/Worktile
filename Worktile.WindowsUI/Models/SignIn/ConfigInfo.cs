﻿namespace Worktile.WindowsUI.Models.SignIn
{
    public class ConfigInfo
    {
        public string Domain { get; set; }
        public string Version { get; set; }
        public string BaseUrl { get; set; }
        public string OuterSiteOrigin { get; set; }
        public UrlBox Box { get; set; }
        public string CdnRoot { get; set; }
        public bool IsIndependent { get; set; }
    }
}