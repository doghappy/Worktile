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

namespace Worktile.Views.Contact.Detail
{
    public sealed partial class MemberDetailPage : Page, INotifyPropertyChanged
    {
        public MemberDetailPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        readonly UserService _userService;
        public event PropertyChangedEventHandler PropertyChanged;

        TethysAvatar Avatar { get; set; }

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

        private bool _isStar;
        public bool IsStar
        {
            get => _isStar;
            set
            {
                if (_isStar != value)
                {
                    _isStar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsStar)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Avatar = e.Parameter as TethysAvatar;
           // Avatar.Resize(AvatarSize.X160);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            Member = await _userService.GetMemberInfoAsync(Avatar.Id);
            string[] uids = await _userService.GetFollowsAsync();
            IsStar = uids.Contains(Member.Uid);
            IsActive = false;
        }
    }
}
