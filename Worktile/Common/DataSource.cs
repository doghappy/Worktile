using System.Collections.Generic;
using Windows.Storage;
using Worktile.Models.Message.Session;
using Worktile.Models.Team;

namespace Worktile.Common
{
    static class DataSource
    {
        public static string SubDomain
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(SubDomain)].ToString();
            set => ApplicationData.Current.LocalSettings.Values[nameof(SubDomain)] = value;
        }

        public static ApiModels.ApiUserMe.Data ApiUserMeData { get; set; }
        public static Team Team { get; set; }

        public static List<ChannelSession> JoinedChannels { get; set; }
    }
}
