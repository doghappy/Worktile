using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class JoinChannelPage : Page, INotifyPropertyChanged
    {
        public JoinChannelPage()
        {
            InitializeComponent();
            _messageService = new MessageService();
            Sessions = new ObservableCollection<ChannelSession>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly MessageService _messageService;
        private List<ChannelSession> _sessions;
        private MasterPage _masterPage;
        private Frame _frame;

        public ObservableCollection<ChannelSession> Sessions { get; }

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

        private bool _canGoBack;
        public bool CanGoBack
        {
            get => _canGoBack;
            set
            {
                if (_canGoBack != value)
                {
                    _canGoBack = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoBack)));
                }
            }
        }

        private string _queryText;
        public string QueryText
        {
            get => _queryText;
            set
            {
                if (_queryText != value)
                {
                    _queryText = value;
                    OnQueryTextChanged();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QueryText)));
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            _sessions = await _messageService.GetAllChannelsAsync();
            _masterPage = this.GetParent<MasterPage>();
            _frame = _masterPage.GetChild<Frame>("MasterContentFrame");
            CanGoBack = _frame.CanGoBack;
            foreach (var item in _sessions)
            {
                item.ForShowAvatar();
                Sessions.Add(item);
            }
            IsActive = false;
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var session = btn.DataContext as ChannelSession;
            var res = await _messageService.ActiveChannelAsync(session.Id);
            if (res)
            {
                _masterPage.InserSession(session);
                GoBack();
            }
        }

        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var session = btn.DataContext as ChannelSession;
            var res = await _messageService.JoinChannelAsync(session.Id);
            if (res)
            {
                _masterPage.InserSession(session);
                GoBack();
            }
        }

        private void OnQueryTextChanged()
        {
            Sessions.Clear();
            string text = QueryText.Trim();
            if (text == string.Empty)
            {
                foreach (var item in _sessions)
                {
                    Sessions.Add(item);
                }
            }
            else if (text != ",")
            {
                foreach (var item in _sessions)
                {
                    if (item.Name.Contains(text, StringComparison.CurrentCultureIgnoreCase) || item.NamePinyin.Contains(text, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Sessions.Add(item);
                    }
                }
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
            else
            {
                _frame.Navigate(typeof(TransparentPage));
            }
        }
    }
}
