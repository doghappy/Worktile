using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.IM.ApiMessages;
using Worktile.ApiModels.IM.ApiPigeonMessages;
using Worktile.Common;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Domain.SocketMessageConverter.Converters;
using Worktile.Enums;
using Worktile.Enums.IM;
using Worktile.Infrastructure;
using Worktile.Models.IM.Message;
using Worktile.ViewModels.Infrastructure;
using Worktile.WtRequestClient;

namespace Worktile.Views.Message
{
    public sealed partial class ChatPage : Page, INotifyPropertyChanged
    {
        public ChatPage()
        {
            InitializeComponent();
            Messages = new ObservableCollection<Message>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Message> Messages { get; }

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

        private MessageDetailToChatNavParam _navParam;
        private string _next;
        private string _latestId;
        private bool? _hasMore;


        private string Url
        {
            get
            {
                int refType = _navParam.Session.Type == SessionType.Channel ? 1 : 2;
                if (_navParam.Session.IsAssistant)
                {
                    string component = null;
                    if (_navParam.Session.Component.HasValue)
                    {
                        component = GetComponentByNumber(_navParam.Session.Component.Value);
                    }
                    string url = $"/api/pigeon/messages?ref_id={_navParam.Session.Id}&ref_type={refType}&filter_type={_navParam.Nav.FilterType}&component={component}&size=20";
                    if (!string.IsNullOrEmpty(_next))
                    {
                        url += "&next=" + _next;
                    }
                    return url;
                }
                else
                {
                    string url = "/api/messages?";
                    if (_navParam.Nav.IsPin)
                    {
                        url += $"session_id={_navParam.Session.Id}&size=20";
                    }
                    else
                    {
                        url += $"ref_id={_navParam.Session.Id}&ref_type={refType}&latest_id={_latestId}&size=20";
                    }
                    return url;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navParam = e.Parameter as MessageDetailToChatNavParam;
            Worktile.MainPage.OnMessageReceived += OnMessageReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Worktile.MainPage.OnMessageReceived -= OnMessageReceived;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMessagesAsync();
            MsgTextBox.Focus(FocusState.Programmatic);
        }

        private async void OnMessageReceived(string json)
        {
            var apiMsg = JsonConvert.DeserializeObject<Models.IM.Message.Message>(json);
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    var member = DataSource.Team.Members.Single(m => m.Uid == apiMsg.From.Uid);
                    Messages.Add(new Message
                    {
                        Avatar = new TethysAvatar
                        {
                            DisplayName = member.DisplayName,
                            Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, FromType.User),
                            Foreground = new SolidColorBrush(Colors.White),
                            Background = AvatarHelper.GetColorBrush(member.DisplayName)
                        },
                        Content = GetContent(apiMsg),
                        Time = apiMsg.CreatedAt,
                        Type = (MessageType)apiMsg.Type
                    });
                    MsgTextBox.Text = string.Empty;
                });
            });
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (_hasMore.HasValue && _hasMore.Value && !IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await LoadMessagesAsync();
            }
        }

        private string GetComponentByNumber(int c)
        {
            string component = null;
            switch (c)
            {
                case 0: component = "message"; break;
                case 1: component = "drive"; break;
                case 2: component = "task"; break;
                case 3: component = "calendar"; break;
                case 4: component = "report"; break;
                case 5: component = "crm"; break;
                case 6: component = "approval"; break;
                case 9: component = "leave"; break;
                case 20: component = "bulletin"; break;
                case 30: component = "appraisal"; break;
                case 40: component = "okr"; break;
                case 50: component = "portal"; break;
                case 60: component = "mission"; break;
            }
            return component;
        }

        private async Task LoadMessagesAsync()
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetJTokenAsync(Url);
            if (_navParam.Session.IsAssistant)
                ReadApiPigeonMessage(data);
            else
                ReadApiMessage(data);
            IsActive = false;
        }

        private void ReadApiPigeonMessage(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiPigeonMessages>();

            foreach (var item in apiData.Data.Messages)
            {
                //item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, AvatarSize.X80, item.From.Type);
                //item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
                //item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);
                //item.Body.Content = Markdown.FormatForMessage(item.Body.Content);

                //if (item.From.Type == FromType.Service)
                //{
                //    item.Body.InlineAttachment.Pretext = Markdown.FormatForMessage(item.Body.InlineAttachment.Pretext);
                //}

                Messages.Insert(0, new Message
                {
                    //Avatar = AvatarHelper.GetTethysAvatar(item.From.Uid, item.From.Type),
                    Avatar = new TethysAvatar
                    {
                        DisplayName = item.From.DisplayName,
                        Source = AvatarHelper.GetAvatarBitmap(item.From.Avatar, AvatarSize.X80, item.From.Type),
                        Background = AvatarHelper.GetColorBrush(item.From.DisplayName)
                    },
                    Content = GetContent(item),
                    Time = item.CreatedAt,
                    Type = (MessageType)item.Type
                });
            }
            _next = apiData.Data.Next;
            _hasMore = apiData.Data.Next != null;
        }

        private void ReadApiMessage(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiMessages>();
            bool flag = false;
            if (Messages.Any())
            {
                apiData.Data.Messages.Reverse();
                flag = true;
            }
            foreach (var item in apiData.Data.Messages)
            {
                //item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, AvatarSize.X80, item.From.Type);
                //item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
                //item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);
                //item.Body.Content = Markdown.FormatForMessage(item.Body.Content);
                //item.Body.InlineAttachment.Pretext = Markdown.FormatForMessage(item.Body.InlineAttachment.Pretext);

                var msg = new Message
                {
                    Avatar = new TethysAvatar
                    {
                        DisplayName = item.From.DisplayName,
                        Source = AvatarHelper.GetAvatarBitmap(item.From.Avatar, AvatarSize.X80, item.From.Type),
                        Foreground = new SolidColorBrush(Colors.White)
                    },
                    Content = GetContent(item),
                    Time = item.CreatedAt,
                    Type = (MessageType)item.Type
                };
                if (Path.GetExtension(item.From.Avatar).ToLower() == ".png")
                    msg.Avatar.Background = new SolidColorBrush(Colors.White);
                else
                    msg.Avatar.Background = AvatarHelper.GetColorBrush(item.From.DisplayName);

                if (flag)
                    Messages.Insert(0, msg);
                else
                    Messages.Add(msg);
            }
            _latestId = apiData.Data.LatestId;
            _hasMore = apiData.Data.More;
        }

        private string GetContent(Models.IM.Message.Message msg)
        {
            switch (msg.Type)
            {
                case Enums.IM.MessageType.Attachment:
                case Enums.IM.MessageType.Calendar:
                    return msg.Body.InlineAttachment.Text;
                default: return msg.Body.Content;
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
                int toType = 1;
                if (_navParam.Session.Type == SessionType.Session)
                    toType = 2;
                var data = new
                {
                    fromType = 1,
                    from = DataSource.ApiUserMe.Uid,
                    to = _navParam.Session.Id,
                    toType,
                    messageType = 1,
                    client = 1,
                    markdown = 1,
                    //attachment = null,
                    content = msg
                };
                await Worktile.MainPage.SendMessageAsync(SocketMessageType.Message, data);
            }
        }
    }
}
