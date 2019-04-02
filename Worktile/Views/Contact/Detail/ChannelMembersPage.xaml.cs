using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Services;
using Worktile.Models.Member;
using System.ComponentModel;
using Worktile.Models.Message.Session;
using System.Collections.ObjectModel;
using Worktile.NavigateModels.Message;

namespace Worktile.Views.Contact.Detail
{
    public sealed partial class ChannelMembersPage : Page, INotifyPropertyChanged
    {
        public ChannelMembersPage()
        {
            InitializeComponent();
            _messageService = new ChannelMessageService();
        }

        readonly ChannelMessageService _messageService;
        public event PropertyChangedEventHandler PropertyChanged;
        private ChannelSession _session;

        TethysAvatar Avatar { get; set; }

        public ObservableCollection<Member> Members { get; set; }

        private Member _member;
        public Member Member
        {
            get => _member;
            set
            {
                if (_member != value)
                {
                    _member = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                    IsPaneOpen = true;
                }
            }
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set
            {
                if (_isPaneOpen != value)
                {
                    _isPaneOpen = value;
                    if (!value)
                    {
                        _member = null;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPaneOpen)));
                }
            }
        }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _session = e.Parameter as ChannelSession;
            foreach (var item in _session.Members)
            {
                item.ForShowAvatar(AvatarSize.X80);
            }
            Members = _session.Members;
            Avatar = _session.TethysAvatar;
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = this.GetParent<LightMainPage>();
            mainPage.IsAppLocked = true;
            mainPage.SelectedApp = mainPage.Apps.First();
            mainPage.ContentFrameNavigate(typeof(Message.MasterPage), new ToMasterDetailParam
            {
                Action = NavigateModels.Message.Action.NewSession,
                Parameter = _session
            });
            mainPage.IsAppLocked = false;
        }
    }
}
