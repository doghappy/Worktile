using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common;
using Worktile.Common.WtRequestClient;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class JoinGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public JoinGroupDialog()
        {
            InitializeComponent();
            Sessions = new ObservableCollection<Session>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<Session> OnSessionActived;

        public ObservableCollection<Session> Sessions { get; }

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

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        List<Session> _sessions;

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            _sessions = new List<Session>();
            var client = new WtHttpClient();
            string url = "/api/channels?type=all&filter=all&status=ok";
            var data = await client.GetAsync<ApiDataResponse<List<Channel>>>(url);
            foreach (var item in data.Data)
            {
                var session = MessageHelper.GetSession(item);
                Sessions.Add(session);
                _sessions.Add(session);
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
                        if (item.DisplayName.ToLower().Contains(text))
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
            var session = btn.DataContext as Session;
            string url = $"/api/channels/{session.Id}/active";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<object>>(url);
            if (data.Code == 200)
            {
                OnSessionActived?.Invoke(session);
                Hide();
            }
        }
    }
}
