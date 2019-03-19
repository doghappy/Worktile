using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;
using Worktile.Common.Communication;
using Worktile.Services;
using System.Collections.ObjectModel;
using Worktile.Common;
using System.Linq;
using Microsoft.Toolkit.Uwp.Helpers;
using Worktile.Models;
using Worktile.Enums;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Enums.Message;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class ChannelMessagePage : Page, INotifyPropertyChanged
    {
        public ChannelMessagePage()
        {
            InitializeComponent();
            _messageService = new MessageService();
            _fileService = new FileService();
            Messages = new ObservableCollection<Models.Message.Message>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly MessageService _messageService;
        readonly FileService _fileService;
        private ChannelSession _session;
        private TopNav _nav;
        const int RefType = 1;

        public ObservableCollection<Models.Message.Message> Messages { get; }

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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WtSocket.OnMessageReceived -= OnMessageReceived;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            var detailPage = this.GetParent<ChannelDetailPage>();
            _session = detailPage.Session;
            _nav = detailPage.SelectedNav;

            await LoadMessagesAsync();
            if (MsgTextBox != null)
                MsgTextBox.Focus(FocusState.Programmatic);
            await _messageService.ClearUnReadAsync(_session.Id);
            WtSocket.OnMessageReceived += OnMessageReceived;
            IsActive = false;
        }

        private async Task LoadMessagesAsync()
        {
            var messages = await _messageService.LoadMessagesAsync(_session, _nav.FilterType);
            bool flag = false;
            if (Messages.Any())
            {
                messages.Reverse();
                flag = true;
            }
            foreach (var item in messages)
            {
                if (flag)
                    Messages.Insert(0, item);
                else
                    Messages.Add(item);
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (_messageService.HasMore.HasValue
                && _messageService.HasMore.Value
                && !IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await LoadMessagesAsync();
            }
        }

        private async void MsgTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (Window.Current.CoreWindow.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down))
                {
                    await SendMessageAsync();
                }
                else
                {
                    int index = MsgTextBox.SelectionStart;
                    MsgTextBox.Text = MsgTextBox.Text.Insert(index, Environment.NewLine);
                    MsgTextBox.SelectionStart = index + 1;
                }
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }

        private async Task SendMessageAsync()
        {
            string msg = MsgTextBox.Text.Trim();
            if (msg != string.Empty)
            {
                var result = await SendMessageAsync(msg);
                if (result)
                {
                    MsgTextBox.Text = string.Empty;
                }
            }
        }

        private void EmojiPicker_OnEmojiSelected(string emoji)
        {
            int index = MsgTextBox.SelectionStart;
            MsgTextBox.Text = MsgTextBox.Text.Insert(index, emoji);
            MsgTextBox.SelectionStart = index + 1;
        }

        private async void AttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.Downloads
            };
            picker.FileTypeFilter.Add("*");
            var files = await picker.PickMultipleFilesAsync();
            await _fileService.UploadFileAsync(files, _session.Id, RefType);
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            bool result = await _messageService.PinAsync(msg.Id, _session);
            if (result)
            {
                msg.IsPinned = true;
            }
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            bool result = await _messageService.UnPinAsync(msg.Id, _session);
            if (result)
            {
                msg.IsPinned = false;
            }
        }

        private async void OnMessageReceived(Models.Message.Message message)
        {
            if (message.To.Id == _session.Id)
            {
                await Task.Run(async () =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        var member = DataSource.Team.Members.Single(m => m.Uid == message.From.Uid);
                        message.From.TethysAvatar = new TethysAvatar
                        {
                            DisplayName = member.DisplayName,
                            Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, FromType.User),
                            Background = AvatarHelper.GetColorBrush(member.DisplayName)
                        };
                        message.IsPinned = false;
                        Messages.Add(message);
                    });
                });
            }
        }

        public async Task<bool> SendMessageAsync(string msg)
        {
            var data = new
            {
                fromType = 1,
                from = DataSource.ApiUserMeData.Me.Uid,
                to = _session.Id,
                toType = 1,
                messageType = 1,
                client = 1,
                markdown = 1,
                content = msg
            };
            return await WtSocket.SendMessageAsync(SocketMessageType.Message, data);
        }
    }
}
