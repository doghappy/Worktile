using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Models;
using Worktile.Services;
using Worktile.Views.Contact.Detail;

namespace Worktile.Views.Contact
{
    public sealed partial class StarListPage : Page, INotifyPropertyChanged
    {
        public StarListPage()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<TethysAvatar>();
            _userService = new UserService();
        }

        readonly UserService _userService;
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<TethysAvatar> Avatars { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            string[] uids = await _userService.GetFollowsAsync();
            var avatars = DataSource.Team.Members
                 .Where(m => uids.Contains(m.Uid))
                 .Select(m => m.TethysAvatar);
            foreach (var item in avatars)
            {
                Avatars.Add(item);
            }
            IsActive = false;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var avatar = e.AddedItems[0] as TethysAvatar;
            var masterPage = this.GetParent<MasterPage>();
            masterPage.ContentFrameNavigate(typeof(MemberDetailPage), avatar);
        }
    }
}
