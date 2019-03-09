using EnumsNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Models;
using Worktile.Models.Member;
using Worktile.Enums;

namespace Worktile.ViewModels.Profile
{
    class ProfileViewModel : ViewModel, INotifyPropertyChanged
    {
        public ProfileViewModel()
        {
            TeamAvatar = DataSource.ApiUserMeData.Config.Box.LogoUrl + DataSource.Team.Logo;
            TeamName = DataSource.Team.Name;
            Version = DataSource.Team.PricingVersion.AsString(EnumFormat.Description);
            UserLimit = DataSource.Team.UserLimit;
            MemberCount = DataSource.Team.MemberCount;
            ExpiredAt = DataSource.Team.ExpiredAt.ToShortDateString();
            MemberTitle = DataSource.ApiUserMeData.Me.Title;
            MemberMobile = DataSource.ApiUserMeData.Me.Mobile;
            MemberEmail = DataSource.ApiUserMeData.Me.Email;
            UserAvatar = AvatarHelper.GetAvatar(new Member
            {
                Avatar = DataSource.ApiUserMeData.Me.Avatar,
                Name = DataSource.ApiUserMeData.Me.Name,
                DisplayName = DataSource.ApiUserMeData.Me.DisplayName
            }, AvatarSize.X320);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _teamAvatar;
        public string TeamAvatar
        {
            get => _teamAvatar;
            set
            {
                if (_teamAvatar != value)
                {
                    _teamAvatar = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TeamName { get; }
        public string Version { get; }
        public int UserLimit { get; }
        public int MemberCount { get; }
        public string ExpiredAt { get; }
        public string MemberTitle { get; }
        public string MemberMobile { get; }
        public string MemberEmail { get; }

        private TethysAvatar _userAvatar;
        public TethysAvatar UserAvatar
        {
            get => _userAvatar;
            set
            {
                if (_userAvatar != value)
                {
                    _userAvatar = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


    }
}
