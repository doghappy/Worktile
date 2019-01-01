using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Worktile.Models.IM;

namespace Worktile.Views.IM
{
    public sealed partial class ChannelChatPage : Page, INotifyPropertyChanged
    {
        public ChannelChatPage()
        {
            InitializeComponent();
            NavItems = new ObservableCollection<NavItem>
            {
                new NavItem { Key = "unread", Name = "未读" },
                new NavItem { Key = "read", Name = "已读" },
                new NavItem { Key = "pending", Name = "待处理" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<NavItem> NavItems { get; }

        private ChatNav _chatNav;
        public ChatNav ChatNav
        {
            get => _chatNav;
            set
            {
                if (_chatNav != value)
                {
                    _chatNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatNav)));
                }
            }
        }

        private NavItem _selectedNav;
        public NavItem SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatNav)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SelectedNav = NavItems.First();
            ChatNav = e.Parameter as ChatNav;
        }
    }

    public class NavItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
