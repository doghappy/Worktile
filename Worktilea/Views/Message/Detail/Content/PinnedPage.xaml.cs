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
using Worktile.Common;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class PinnedPage : Page, INotifyPropertyChanged
    {
        public PinnedPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private MessageService _messageService;
        private ISession _session;
        private string _anchor;

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

        private IncrementalCollection<Models.Message.Message> _messages;
        public IncrementalCollection<Models.Message.Message> Messages
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var masterPage = this.GetParent<MasterPage>();
            _session = masterPage.SelectedSession;
            switch (_session.PageType)
            {
                case PageType.Member:
                    _messageService = new MessageService();
                    break;
                case PageType.Channel:
                    _messageService = new ChannelMessageService();
                    break;
                default:
                    throw new InvalidOperationException();
            }
            Messages = new IncrementalCollection<Models.Message.Message>(LoadMessagesAsync);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var msg = btn.DataContext as Models.Message.Message;
            bool result = await _messageService.UnPinAsync(msg.Id, _session);
            if (result)
            {
                Messages.Remove(msg);
            }
        }

        private async Task<IEnumerable<Models.Message.Message>> LoadMessagesAsync()
        {
            IsActive = true;
            const int SIZE = 10;
            var list = new List<Models.Message.Message>();
            var data = await _messageService.GetPinnedMessagesAsync(_session, _anchor);
            if (data.Code == 200 && data.Data.Pinneds.Any())
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
                        Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, item.Reference.From.Type)
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
    }
}
