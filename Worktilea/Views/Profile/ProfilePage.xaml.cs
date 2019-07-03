using EnumsNET;
using System.ComponentModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Views.SignIn;
using Worktile.Models.Member;

namespace Worktile.Views.Profile
{
    public sealed partial class ProfilePage : Page, INotifyPropertyChanged
    {
        public ProfilePage()
        {
            InitializeComponent();
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TeamAvatar)));
                }
            }
        }

        private string _teamName;
        public string TeamName
        {
            get => _teamName;
            set
            {
                if (_teamName != value)
                {
                    _teamName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TeamName)));
                }
            }
        }

        private string _version;
        public string Version
        {
            get => _version;
            set
            {
                if (_version != value)
                {
                    _version = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Version)));
                }
            }
        }

        private int _userLimit;
        public int UserLimit
        {
            get => _userLimit;
            set
            {
                if (_userLimit != value)
                {
                    _userLimit = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserLimit)));
                }
            }
        }

        private int _memberCount;
        public int MemberCount
        {
            get => _memberCount;
            set
            {
                if (_memberCount != value)
                {
                    _memberCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberCount)));
                }
            }
        }

        private string _expiredAt;
        public string ExpiredAt
        {
            get => _expiredAt;
            set
            {
                if (_expiredAt != value)
                {
                    _expiredAt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpiredAt)));
                }
            }
        }

        private string _memberTitle;
        public string MemberTitle
        {
            get => _memberTitle;
            set
            {
                if (_memberTitle != value)
                {
                    _memberTitle = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberTitle)));
                }
            }
        }

        private string _memberMobile;
        public string MemberMobile
        {
            get => _memberMobile;
            set
            {
                if (_memberMobile != value)
                {
                    _memberMobile = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberMobile)));
                }
            }
        }

        private string _memberEmail;
        public string MemberEmail
        {
            get => _memberEmail;
            set
            {
                if (_memberEmail != value)
                {
                    _memberEmail = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberEmail)));
                }
            }
        }

        private TethysAvatar _userAvatar;
        public TethysAvatar UserAvatar
        {
            get => _userAvatar;
            set
            {
                if (_userAvatar != value)
                {
                    _userAvatar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserAvatar)));
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values.Remove("Domain");
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(PasswordSignInPage));
        }
    }
}
