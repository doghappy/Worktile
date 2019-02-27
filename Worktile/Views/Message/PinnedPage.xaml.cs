using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiPinnedMessages;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Domain.MessageContentReader;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message;

namespace Worktile.Views.Message
{
    public sealed partial class PinnedPage : Page, INotifyPropertyChanged
    {
        public PinnedPage()
        {
            InitializeComponent();
            Messages = new IncrementalCollection<Models.Message.Message>(LoadMessagesAsync);
        }

        Session _session;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public IncrementalCollection<Models.Message.Message> Messages { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _session = e.Parameter as Session;
        }

        string _anchor;

        string IdType => _session.Type == SessionType.Channel ? "channel_id" : "session_id";

        private async Task<IEnumerable<Models.Message.Message>> LoadMessagesAsync()
        {
            IsActive = true;
            const int SIZE = 10;
            var list = new List<Models.Message.Message>();
            string url = $"/api/pinneds?{IdType}={_session.Id}&anchor={_anchor}&size={SIZE}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiPinnedMessages>(url);
            if (data.Data.Pinneds.Any())
            {
                _anchor = data.Data.Pinneds.Last().Id;
                if (data.Data.Batch < SIZE)
                {
                    Messages.HasMoreItems = false;
                }
                foreach (var item in data.Data.Pinneds)
                {
                    IMemberBase member = null;
                    if (item.Reference.From.Type == FromType.User)
                        member = DataSource.Team.Members.Single(m => m.Uid == item.Reference.From.Uid);
                    else if (item.Reference.From.Type == FromType.Service)
                        member = DataSource.Team.Services.Single(m => m.ServiceId == item.Reference.From.Uid);

                    item.Reference.From.TethysAvatar = new TethysAvatar
                    {
                        DisplayName = member.DisplayName,
                        Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, item.Reference.From.Type),
                        Foreground = new SolidColorBrush(Colors.White)
                    };
                    if (Path.GetExtension(member.Avatar).ToLower() == ".png")
                        item.Reference.From.TethysAvatar.Background = new SolidColorBrush(Colors.White);
                    else
                        item.Reference.From.TethysAvatar.Background = AvatarHelper.GetColorBrush(member.DisplayName);
                    list.Add(item.Reference);
                }
            }
            else
            {
                Messages.HasMoreItems = false;
            }
            IsActive = false;
            return list;
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var msg = btn.DataContext as Models.Message.Message;
            string url = $"/api/messages/{msg.Id}/unpinned?{IdType}={_session.Id}";
            var client = new WtHttpClient();
            var response = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                Messages.Remove(msg);
            }
        }
    }
}
