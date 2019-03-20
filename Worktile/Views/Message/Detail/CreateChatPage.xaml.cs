using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Services;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class CreateChatPage : Page, INotifyPropertyChanged
    {
        public CreateChatPage()
        {
            InitializeComponent();
            _messageService = new MessageService();
            Avatars = new ObservableCollection<TethysAvatar>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MessageService _messageService;
        private MasterPage _masterPage;

        public ObservableCollection<TethysAvatar> Avatars { get; }

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _masterPage = this.GetParent<MasterPage>();
            foreach (var item in DataSource.Team.Members)
            {
                if (item.IsTrueMember())
                {
                    Avatars.Add(item.TethysAvatar);
                }
            }
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            var btn = sender as Button;
            var avatar = btn.DataContext as TethysAvatar;
            var session = await _messageService.CreateSessionAsync(avatar.Id);
            session.ForShowAvatar(AvatarSize.X80);
            _masterPage.InserSession(session, true);
            _masterPage.SelectedSession = session;
            IsActive = false;
        }

        private void OnQueryTextChanged()
        {
            Avatars.Clear();
            string text = QueryText.Trim();
            if (text == string.Empty)
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember())
                    {
                        Avatars.Add(item.TethysAvatar);
                    }
                }
            }
            else if (text != ",")
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember() && (item.TethysAvatar.Name.Contains(text, StringComparison.CurrentCultureIgnoreCase) || item.DisplayNamePinyin.Contains(text, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        Avatars.Add(item.TethysAvatar);
                    }
                }
            }
        }
    }
}
