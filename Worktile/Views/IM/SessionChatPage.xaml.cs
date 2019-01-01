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
    public sealed partial class SessionChatPage : Page, INotifyPropertyChanged
    {
        public SessionChatPage()
        {
            InitializeComponent();
            NavItems = new ObservableCollection<NavItem>
            {
                new NavItem { Key = "unread", Name = "消息" },
                new NavItem { Key = "read", Name = "文件" },
                new NavItem { Key = "pending", Name = "固定消息" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<NavItem> NavItems { get; }

        private ChatNav ChatNav;
        public ChatNav MyProperty
        {
            get => ChatNav;
            set
            {
                if (ChatNav != value)
                {
                    ChatNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyProperty)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyProperty)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ChatNav = e.Parameter as ChatNav;
            SelectedNav = NavItems.First();
        }
    }
}
