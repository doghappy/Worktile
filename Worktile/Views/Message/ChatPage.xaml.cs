using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.Message.ApiMessages;
using Worktile.ApiModels.Message.ApiPigeonMessages;
using Worktile.Common;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Enums;
using Worktile.Common.WtRequestClient;
using Windows.Storage.Pickers;
using System.Net.Http;
using Worktile.ApiModels.Upload;
using Windows.Storage.AccessCache;
using Worktile.ApiModels;
using Worktile.Views.Message.NavigationParam;
using Worktile.ViewModels.Message.Session;

namespace Worktile.Views.Message
{
    public sealed partial class ChatPage : Page, INotifyPropertyChanged
    {
        public ChatPage()
        {
            InitializeComponent();
            //Messages = new ObservableCollection<Message>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //public ObservableCollection<Message> Messages { get; }

        private SessionMessageViewModel _viewModel;
        private SessionMessageViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
                }
            }
        }

        private ToUnReadMsgPageParam _navParam;
        //private string _next;
        //private string _latestId;
        //private bool? _hasMore;

        //private string Url
        //{
        //    get
        //    {
        //        if (_navParam.Session.IsAssistant)
        //        {
        //            string component = null;
        //            if (_navParam.Session.Component.HasValue)
        //            {
        //                component = GetComponentByNumber(_navParam.Session.Component.Value);
        //            }
        //            string url = $"/api/pigeon/messages?ref_id={_navParam.Session.Id}&ref_type={_navParam.Session.RefType}&filter_type={_navParam.Nav.FilterType}&component={component}&size=20";
        //            if (!string.IsNullOrEmpty(_next))
        //            {
        //                url += "&next=" + _next;
        //            }
        //            return url;
        //        }
        //        else
        //        {
        //            string url = "/api/messages?";
        //            if (_navParam.Nav.IsPin)
        //            {
        //                url += $"session_id={_navParam.Session.Id}&size=20";
        //            }
        //            else
        //            {
        //                url += $"ref_id={_navParam.Session.Id}&ref_type={_navParam.Session.RefType}&latest_id={_latestId}&size=20";
        //            }
        //            return url;
        //        }
        //    }
        //}

        //private bool _isAssistant;
        //public bool IsAssistant
        //{
        //    get => _isAssistant;
        //    set
        //    {
        //        if (_isAssistant != value)
        //        {
        //            _isAssistant = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAssistant)));
        //        }
        //    }
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navParam = e.Parameter as ToUnReadMsgPageParam;
            ViewModel = new SessionMessageViewModel(_navParam.Session);
            _navParam.MainViewModel.OnMessageReceived += ViewModel.OnMessageReceived;
            //IsAssistant = _navParam.Session.IsAssistant;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navParam.MainViewModel.OnMessageReceived -= ViewModel.OnMessageReceived;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
            if (MsgTextBox != null)
                MsgTextBox.Focus(FocusState.Programmatic);
            await ViewModel.ClearUnReadAsync();
        }

        //private async Task ClearUnReadAsync()
        //{
        //    if (_navParam.Session.IsAssistant)
        //    {
        //        string url = $"/api/unreads/{_navParam.Session.Id}/messages-unreads?socket_id=";
        //        var client = new WtHttpClient();
        //        await client.PutAsync<object>(url);
        //    }
        //    else
        //    {
        //        string url = $"/api/messages/unread/clear?ref_id={_navParam.Session.Id}";
        //        var client = new WtHttpClient();
        //        await client.PutAsync<object>(url);
        //    }
        //    _navParam.MainPage.UnreadBadge -= _navParam.Session.UnRead;
        //    await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => _navParam.Session.UnRead = 0));
        //}

        //private async void OnMessageReceived(Models.Message.Message apiMsg)
        //{
        //    if (apiMsg.To.Id == _navParam.Session.Id)
        //    {
        //        await Task.Run(async () =>
        //        {
        //            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
        //            {
        //                var member = DataSource.Team.Members.Single(m => m.Uid == apiMsg.From.Uid);
        //                Messages.Add(new Message
        //                {
        //                    Id = apiMsg.Id,
        //                    Avatar = new TethysAvatar
        //                    {
        //                        DisplayName = member.DisplayName,
        //                        Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, FromType.User),
        //                        Foreground = new SolidColorBrush(Colors.White),
        //                        Background = AvatarHelper.GetColorBrush(member.DisplayName)
        //                    },
        //                    Content = MessageHelper.GetContent(apiMsg),
        //                    Time = apiMsg.CreatedAt,
        //                    IsPinned = false,
        //                    Type = apiMsg.Type
        //                });
        //            });
        //        });
        //    }
        //}

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (ViewModel.HasMore.HasValue && ViewModel.HasMore.Value && !ViewModel.IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await ViewModel.LoadMessagesAsync();
            }
        }

        

        //private async Task LoadMessagesAsync()
        //{
        //    IsActive = true;
        //    var client = new WtHttpClient();
        //    var data = await client.GetJTokenAsync(Url);
        //    if (_navParam.Session.IsAssistant)
        //        ReadApiPigeonMessage(data);
        //    else
        //        ReadApiMessage(data);
        //    IsActive = false;
        //}

        //private void ReadApiPigeonMessage(JToken jToken)
        //{
        //    var apiData = jToken.ToObject<ApiPigeonMessages>();

        //    foreach (var item in apiData.Data.Messages)
        //    {
        //        Messages.Insert(0, new Message
        //        {
        //            Id = item.Id,
        //            Avatar = new TethysAvatar
        //            {
        //                DisplayName = item.From.DisplayName,
        //                Source = AvatarHelper.GetAvatarBitmap(item.From.Avatar, AvatarSize.X80, item.From.Type),
        //                Background = AvatarHelper.GetColorBrush(item.From.DisplayName)
        //            },
        //            Content = MessageHelper.GetContent(item),
        //            Time = item.CreatedAt,
        //            Type = item.Type,
        //            IsPinned = item.IsPinned
        //        });
        //    }
        //    _next = apiData.Data.Next;
        //    _hasMore = apiData.Data.Next != null;
        //}

        //private void ReadApiMessage(JToken jToken)
        //{
        //    var apiData = jToken.ToObject<ApiMessages>();
        //    bool flag = false;
        //    if (Messages.Any())
        //    {
        //        apiData.Data.Messages.Reverse();
        //        flag = true;
        //    }
        //    foreach (var item in apiData.Data.Messages)
        //    {
        //        var msg = new Message
        //        {
        //            Id = item.Id,
        //            Avatar = new TethysAvatar
        //            {
        //                DisplayName = item.From.DisplayName,
        //                Source = AvatarHelper.GetAvatarBitmap(item.From.Avatar, AvatarSize.X80, item.From.Type),
        //                Foreground = new SolidColorBrush(Colors.White)
        //            },
        //            Content = MessageHelper.GetContent(item),
        //            Time = item.CreatedAt,
        //            Type = item.Type,
        //            IsPinned = item.IsPinned
        //        };
        //        if (Path.GetExtension(item.From.Avatar).ToLower() == ".png")
        //            msg.Avatar.Background = new SolidColorBrush(Colors.White);
        //        else
        //            msg.Avatar.Background = AvatarHelper.GetColorBrush(item.From.DisplayName);

        //        if (flag)
        //            Messages.Insert(0, msg);
        //        else
        //            Messages.Add(msg);
        //    }
        //    _latestId = apiData.Data.LatestId;
        //    _hasMore = apiData.Data.More;
        //}

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
                await ViewModel.SendMessageAsync(msg);
                MsgTextBox.Text = string.Empty;
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
            await ViewModel.UploadFileAsync(files);
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Message;
            await ViewModel.PinMessageAsync(msg);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Message;
            await ViewModel.UnPinMessageAsync(msg);
        }
    }
}
