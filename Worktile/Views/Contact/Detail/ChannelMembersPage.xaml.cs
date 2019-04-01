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

namespace Worktile.Views.Contact.Detail
{
    public sealed partial class ChannelMembersPage : Page, INotifyPropertyChanged
    {
        public ChannelMembersPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        readonly UserService _userService;
        public event PropertyChangedEventHandler PropertyChanged;

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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPaneOpen)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var session = e.Parameter as ChannelSession;
            foreach (var item in session.Members)
            {
                item.ForShowAvatar(AvatarSize.X80);
            }
            Members = session.Members;
            Avatar = session.TethysAvatar;
        }
    }
}
