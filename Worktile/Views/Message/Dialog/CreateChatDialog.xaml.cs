using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Message;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class CreateChatDialog : ContentDialog
    {
        public CreateChatDialog()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<TethysAvatar>();
            AddAllMember();
        }

        public event Action<Session> OnSessionCreated;

        public ObservableCollection<TethysAvatar> Avatars { get; }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var avatar = btn.DataContext as TethysAvatar;
            var client = new WtHttpClient();
            var data = await client.PostAsync<ApiDataResponse<ApiModels.ApiTeamChats.Session>>("/api/session", new { uid = avatar.Id });
            if (data.Code == 200)
            {
                OnSessionCreated?.Invoke(MessageHelper.GetSession(data.Data, AvatarSize.X80));
                Hide();
            }
        }

        private void AddAllMember()
        {
            foreach (var item in DataSource.Team.Members)
            {
                if (item.Role != 5)
                    Avatars.Add(AvatarHelper.GetAvatar(item, AvatarSize.X40));
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string text = sender.Text.Trim().ToLower();
                Avatars.Clear();
                if (text == string.Empty)
                {
                    AddAllMember();
                }
                else
                {
                    foreach (var item in DataSource.Team.Members)
                    {
                        if (item.DisplayName.ToLower().Contains(text) || item.DisplayNamePinyin.Contains(text))
                        {
                            if (item.Role != 5)
                                Avatars.Add(AvatarHelper.GetAvatar(item, AvatarSize.X40));
                        }
                    }
                }
            }
        }
    }
}
