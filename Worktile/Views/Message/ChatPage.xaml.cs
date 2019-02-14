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
using Worktile.Enums;
using Worktile.Enums.IM;
using Worktile.Infrastructure;
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
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMessagesAsync();
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
                    Content = item.Body.Content,
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
                        Background = AvatarHelper.GetColorBrush(item.From.DisplayName)
                    },
                    Content = item.Body.Content,
                    Time = item.CreatedAt,
                    Type = (MessageType)item.Type
                };

                if (flag)
                    Messages.Insert(0, msg);
                else
                    Messages.Add(msg);
            }
            _latestId = apiData.Data.LatestId;
            _hasMore = apiData.Data.More;
        }
    }
}
