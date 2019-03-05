using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail.Content;
using Worktile.Views.Message.Detail.Content.Pin;
using Worktile.Common.Extensions;
using Worktile.Enums;

namespace Worktile.ViewModels.Message
{
    class ChannelDetailViewModel : SessionDetailViewModel<ChannelSession>, INotifyPropertyChanged
    {
        public ChannelDetailViewModel(ChannelSession session, Frame contentFrame, MainViewModel mainViewModel)
            : base(session, contentFrame, mainViewModel) { }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<TopNav> Navs { get; protected set; }

        public override string PaneTitle => "群组成员";

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
    }
}
