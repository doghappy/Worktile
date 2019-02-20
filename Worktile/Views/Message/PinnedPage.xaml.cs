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
using Worktile.Enums;
using Worktile.Models;

namespace Worktile.Views.Message
{
    public sealed partial class PinnedPage : Page, INotifyPropertyChanged
    {
        public PinnedPage()
        {
            InitializeComponent();
            Messages = new IncrementalCollection<Message>(LoadMessagesAsync);
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

        public IncrementalCollection<Message> Messages { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _session = e.Parameter as Session;
        }

        string _anchor;

        string IdType => _session.Type == SessionType.Channel ? "channel_id" : "session_id";

        private async Task<IEnumerable<Message>> LoadMessagesAsync()
        {
            IsActive = true;
            const int SIZE = 10;
            var list = new List<Message>();
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
                    var msg = new Message
                    {
                        Id = item.Reference.Id,
                        Avatar = new TethysAvatar
                        {
                            DisplayName = member.DisplayName,
                            Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, item.Reference.From.Type),
                            Foreground = new SolidColorBrush(Colors.White)
                        },
                        Content = ChatPage.GetContent(item.Reference),
                        Time = item.Reference.CreatedAt,
                        Type = item.Reference.Type
                    };
                    if (Path.GetExtension(member.Avatar).ToLower() == ".png")
                        msg.Avatar.Background = new SolidColorBrush(Colors.White);
                    else
                        msg.Avatar.Background = AvatarHelper.GetColorBrush(member.DisplayName);
                    list.Add(msg);
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
            var msg = btn.DataContext as Message;
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
