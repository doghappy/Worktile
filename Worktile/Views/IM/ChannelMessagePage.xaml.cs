using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models.IM;
using Worktile.Models.IM.Message;

namespace Worktile.Views.IM
{
    public sealed partial class ChannelMessagePage : Page, INotifyPropertyChanged
    {
        public ChannelMessagePage()
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
                    //ContentFrame.Navigate(value.SourcePageType);
                    if (value.Messages == null)
                    {
                        value.Messages = new IncrementalCollection<Message>(LoadMessagesAsync);
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SelectedNav = NavItems.First();
            ChatNav = e.Parameter as ChatNav;
        }

        private async Task<IEnumerable<Message>> LoadMessagesAsync()
        {
            await Task.Delay(1000);
            return new List<Message>
            {
                new Message
                {
                    From = new MessageFrom
                    {
                        DisplayName = "DisplayName"
                    }
                }
            };
        }
    }

    public class NavItem : INotifyPropertyChanged
    {
        //public NavItem()
        //{
        //    Messages = new IncrementalCollection<Message>();
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public string Key { get; set; }
        public string Name { get; set; }
        public Type SourcePageType { get; set; }

        private IncrementalCollection<Message> _messages;
        public IncrementalCollection<Message> Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
                }
            }
        }
    }
}
