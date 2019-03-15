using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using Worktile.Models.Message.Session;
using Worktile.Models.Team;

namespace Worktile.Common
{
    static class DataSource
    {
        static DataSource()
        {
            JoinedChannels = new ObservableCollection<ChannelSession>();
        }

        public static string SubDomain
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(SubDomain)].ToString();
            set => ApplicationData.Current.LocalSettings.Values[nameof(SubDomain)] = value;
        }

        public static ApiModels.ApiUserMe.Data ApiUserMeData { get; set; }
        public static Team Team { get; set; }

        public static ObservableCollection<ChannelSession> JoinedChannels { get; }
    }
}
