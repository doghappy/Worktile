using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail.Content;
using Worktile.Views.Message.Detail.Content.Pin;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Common;
using System;
using Worktile.Enums.Privileges;
using System.Threading.Tasks;
using Worktile.Common.WtRequestClient;
using Worktile.ApiModels;

namespace Worktile.ViewModels.Message
{
    class ChannelDetailViewModel : SessionDetailViewModel<ChannelSession>, INotifyPropertyChanged
    {
        public ChannelDetailViewModel(ChannelSession session, Frame contentFrame, MainViewModel mainViewModel, MasterViewModel masterViewModel)
            : base(session, contentFrame, mainViewModel)
        {
            _masterViewModel = masterViewModel;
            SetPermission();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly MasterViewModel _masterViewModel;

        public override ObservableCollection<TopNav> Navs { get; protected set; }

        public override string PaneTitle => "群组成员";

        public bool CanAddService { get; private set; }
        public bool CanAddMember { get; private set; }
        public bool CanAdminChannel { get; private set; }

        private TopNav _selectedNav;
        public override TopNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    int index = Navs.IndexOf(value);
                    switch (index)
                    {
                        case 0:
                            ContentFrame.Navigate(typeof(ChannelMessagePage), new ToUnReadMsgPageParam
                            {
                                Session = Session,
                                Nav = value,
                                MainViewModel = MainViewModel
                            });
                            break;
                        case 1:
                            ContentFrame.Navigate(typeof(FilePage), Session);
                            break;
                        case 2:
                            ContentFrame.Navigate(typeof(ChannelPinnedPage), Session);
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void LoadMembersAvatar()
        {
            IsActive = true;
            var member = Session.Members.FirstOrDefault();
            if (member != null && member.TethysAvatar == null)
            {
                foreach (var item in Session.Members)
                {
                    item.ForShowAvatar(AvatarSize.X80);
                }
            }
            IsActive = false;
        }

        private void SetPermission()
        {
            int adminPermission = Convert.ToInt32(DataSource.Team.Privileges.Admin.Value, 2);
            CanAddService = (adminPermission & (int)AdminPrivilege.Service) != 0;

            int messagePermission = Convert.ToInt32(DataSource.Team.Privileges.Message.Value, 2);
            CanAddMember = Session.Visibility == WtVisibility.Private || (((int)MessagePrivilege.AdminPublicChannel & messagePermission) != 0 && !Session.IsSystem);
            CanAdminChannel = Session.Visibility == WtVisibility.Private || (!Session.IsSystem && ((int)MessagePrivilege.AdminPublicChannel & messagePermission) != 0);
        }

        public async Task ExitChannelAsync()
        {
            string url = $"api/channels/{Session.Id}/leave";
            var res = await WtHttpClient.PutAsync<ApiDataResponse<bool>>(url);
            if (res.Code == 200 && res.Data)
            {
                _masterViewModel.Sessions.Remove(Session);
            }
        }
    }
}
