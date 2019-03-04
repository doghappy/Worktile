using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common.WtRequestClient;
using Worktile.Models.Message.Session;
using Worktile.Common.Extensions;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class JoinGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public JoinGroupDialog()
        {
            InitializeComponent();
            Sessions = new ObservableCollection<ChannelSession>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<ChannelSession> OnActived;
        public event Action<ChannelSession> OnJoined;

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

        List<ChannelSession> _sessions;

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            _sessions = new List<ChannelSession>();
            var client = new WtHttpClient();
            string url = "/api/channels?type=all&filter=all&status=ok";
            var data = await client.GetAsync<ApiDataResponse<List<ChannelSession>>>(url);
            foreach (var item in data.Data)
            {
                item.ForShowAvatar();
                Sessions.Add(item);
                _sessions.Add(item);
            }
            IsActive = false;
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string text = sender.Text.Trim().ToLower();
                Sessions.Clear();
                if (text == string.Empty)
                {
                    foreach (var item in _sessions)
                    {
                        Sessions.Add(item);
                    }
                }
                else
                {
                    foreach (var item in _sessions)
                    {
                        if (item.NamePinyin.ToLower().Contains(text))
                        {
                            Sessions.Add(item);
                        }
                        else if (text != "," && item.NamePinyin.Contains(text))
                        {
                            Sessions.Add(item);
                        }
                    }
                }
            }
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var session = btn.DataContext as ChannelSession;
            string url = $"/api/channels/{session.Id}/active";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<object>>(url);
            if (data.Code == 200)
            {
                OnActived?.Invoke(session);
                Hide();
            }
        }

        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var session = btn.DataContext as ChannelSession;
            string url = $"/api/channels/{session.Id}/join";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                OnJoined?.Invoke(session);
                Hide();
            }
        }
    }
}
