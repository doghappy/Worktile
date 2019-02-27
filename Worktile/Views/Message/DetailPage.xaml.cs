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
using Worktile.Views.Message.NavigationParam;

namespace Worktile.Views.Message
{
    public sealed partial class DetailPage : Page, INotifyPropertyChanged
    {
        public DetailPage()
        {
            InitializeComponent();
            Navs = new ObservableCollection<TopNav>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ToMessageDetailPageParam _navParam;

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
                    int index = Navs.IndexOf(value);
                    if (value.Name == "未读" || value.Name == "已读" || value.Name == "待处理")
                    {
                        ContentFrame.Navigate(typeof(Assistant.AssistantMessagePage), new ToUnReadMsgPageParam
                        {
                            Session = Session,
                            Nav = value,
                            MainViewModel = _navParam.MainViewModel
                        });
                    }
                    else if (value.Name == "消息")
                    {
                        ContentFrame.Navigate(typeof(ChatPage), new ToUnReadMsgPageParam
                        {
                            Session = Session,
                            Nav = value,
                            MainViewModel = _navParam.MainViewModel
                        });
                    }
                    else if (value.Name == "文件")
                    {
                        ContentFrame.Navigate(typeof(FilePage), Session);
                    }
                    else if (value.Name == "固定消息")
                    {
                        ContentFrame.Navigate(typeof(PinnedPage), Session);
                    }
                    else if (value.Name == "已读")
                    {

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
            _navParam = e.Parameter as ToMessageDetailPageParam;
            Session = _navParam.Session;
        }
    }
}
