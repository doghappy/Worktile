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

namespace Worktile.Views.Message
{
    public sealed partial class MessageDetailPage : Page, INotifyPropertyChanged
    {
        public MessageDetailPage()
        {
            InitializeComponent();
            Navs = new ObservableCollection<TopNav>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Session _session;
        public Session Session
        {
            get => _session;
            set
            {
                if (_session != value)
                {
                    _session = value;
                    if (value.IsAssistant)
                    {
                        Navs.Add(new TopNav { Name = "未读", FilterType = 2 });
                        Navs.Add(new TopNav { Name = "已读", FilterType = 4 });
                        Navs.Add(new TopNav { Name = "待处理", FilterType = 3 });
                    }
                    else
                    {
                        Navs.Add(new TopNav { Name = "消息" });
                        Navs.Add(new TopNav { Name = "文件" });
                        Navs.Add(new TopNav { Name = "固定消息", IsPin = true });
                    }
                    SelectedNav = Navs.First();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Session)));
                }
            }
        }

        private TopNav _selectedNav;
        public TopNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    if (Navs.IndexOf(value) == 0)
                    {
                        ContentFrame.Navigate(typeof(ChatPage), new MessageDetailToChatNavParam
                        {
                            Session = Session,
                            Nav = value
                        });
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                }
            }
        }

        public ObservableCollection<TopNav> Navs { get; }

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
            Session = e.Parameter as Session;
        }
    }
}
