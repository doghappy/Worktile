﻿using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModels.ApiTeam;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Domain.SocketMessageConverter.Converters;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Views;
using Worktile.Views.Message;

namespace Worktile.ViewModels
{
    public class MainViewModel : ViewModel, INotifyPropertyChanged, IDisposable
    {
        public MainViewModel(CoreDispatcher dispatcher, Frame contentFrame, InAppNotification inAppNotification)
        {
            Apps = new ObservableCollection<WtApp>();
            _dispatcher = dispatcher;
            _contentFrame = contentFrame;
            _inAppNotification = inAppNotification;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MessageWebSocket _socket;
        public event Action<Models.Message.Message> OnMessageReceived;
        public event Action<Models.Message.Feed> OnFeedReceived;

        private CoreDispatcher _dispatcher;
        private Frame _contentFrame;
        private InAppNotification _inAppNotification;

        public string SocketId { get; private set; }

        private int _unreadBadge;
        public int UnreadBadge
        {
            get => _unreadBadge;
            set
            {
                if (_unreadBadge != value || value == 0)
                {
                    _unreadBadge = value;
                    if (value > 0)
                        UpdateBadgeNumber(value);
                    else
                        BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                }
            }
        }

        public CoreWindowActivationState WindowActivationState { get; set; }

        public ObservableCollection<WtApp> Apps { get; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _bgImage;
        public BitmapImage BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                OnPropertyChanged();
            }
        }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set
            {
                if (_selectedApp != value)
                {
                    _selectedApp = value;
                    OnPropertyChanged();
                    if (value != null)
                    {
                        ContentFrameNavigate(value.Name);
                    }
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task RequestApiUserMeAsync()
        {
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>("/api/user/me");
            DataSource.ApiUserMeData = me.Data;
            DisplayName = DataSource.ApiUserMeData.Me.DisplayName;

            await ConnectSocketAsync();

            string bgImg = DataSource.ApiUserMeData.Me.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BgImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = DataSource.ApiUserMeData.Config.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                byte[] buffer = await client.GetByteArrayAsync(imgUriString);
                BgImage = await ImageHelper.GetImageFromBytesAsync(buffer);
            }
        }

        public async Task RequestApiTeamAsync()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>("/api/team");
            DataSource.Team = data.Data;
            Apps.Add(DataSource.Apps.SingleOrDefault(a => a.Name == "message"));
            DataSource.Team.Apps.ForEach(app =>
            {
                var item = DataSource.Apps.Single(a => a.Name == app.Name);
                Apps.Add(item);
            });
            SelectedApp = Apps.First();
            Logo = new BitmapImage(new Uri(DataSource.ApiUserMeData.Config.Box.LogoUrl + DataSource.Team.Logo));
        }

        private async Task ConnectSocketAsync()
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    _socket = new MessageWebSocket();
                    _socket.Control.MessageType = Windows.Networking.Sockets.SocketMessageType.Utf8;
                    _socket.MessageReceived += Socket_MessageReceived;

                    Uri uri = new Uri($"wss://im.worktile.com/socket.io/?token={DataSource.ApiUserMeData.Me.ImToken}&uid={DataSource.ApiUserMeData.Me.Uid}&client=web&EIO=3&transport=websocket");
                    await _socket.ConnectAsync(uri);

                    using (var dataWriter = new DataWriter(_socket.OutputStream))
                    {
                        string msg = $"40/message?token={DataSource.ApiUserMeData.Me.ImToken}&uid={DataSource.ApiUserMeData.Me.Uid}&client=web";
                        dataWriter.WriteString(msg);
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    KeepConnection();
                });
            });
        }

        private void Socket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            using (DataReader dataReader = args.GetDataReader())
            {
                dataReader.UnicodeEncoding = UnicodeEncoding.Utf8;
                string rawMsg = dataReader.ReadString(dataReader.UnconsumedBufferLength);
                var (msg, cvt) = SocketMessageConverter.Read(rawMsg);
                if (cvt != null)
                {
                    var cvtType = cvt.GetType();
                    switch (cvtType.Name)
                    {
                        case nameof(OpenConverter):
                            SocketId = msg;
                            break;
                        case nameof(MessageConverter):
                            MessageReceived(msg);
                            break;
                        case nameof(FeedConverter):
                            FeedReceived(msg);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 心跳机制，防止服务端断开连接。
        /// </summary>
        private void KeepConnection()
        {
            var timer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    using (var dataWriter = new DataWriter(_socket.OutputStream))
                    {
                        dataWriter.WriteString("2");
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }
                });

            }, TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// 接收解析到的消息，以提供Toast通知和Tile Badge功能
        /// </summary>
        /// <param name="msg"></param>
        private void MessageReceived(string msg)
        {
            var apiMsg = JsonConvert.DeserializeObject<Models.Message.Message>(msg);
            OnMessageReceived?.Invoke(apiMsg);
            if (apiMsg.From.Uid != DataSource.ApiUserMeData.Me.Uid)
            {
                UnreadBadge += 1;
                if (WindowActivationState == CoreWindowActivationState.Deactivated)
                {
                    SendToast(apiMsg);
                }
                else
                {
                    if (Apps.IndexOf(SelectedApp) != 0)
                    {
                        SendToast(apiMsg);
                    }
                }
            }
        }

        private void FeedReceived(string msg)
        {
            var feed = JsonConvert.DeserializeObject<Models.Message.Feed>(msg);
            OnFeedReceived?.Invoke(feed);
        }

        public void UpdateBadgeNumber(int number)
        {
            // Get the blank badge XML payload for a badge number
            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);

            // Set the value of the badge in the XML to our number
            XmlElement badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement.SetAttribute("value", number.ToString());

            // Create the badge notification
            BadgeNotification badge = new BadgeNotification(badgeXml);

            // Create the badge updater for the application
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();

            // And update the badge
            badgeUpdater.Update(badge);
        }

        private void SendToast(Models.Message.Message apiMsg)
        {
            var member = DataSource.Team.Members.Single(m => m.Uid == apiMsg.From.Uid);
            string avatar = "Assets/StoreLogo.scale-400.png";
            if (member.Avatar != string.Empty)
            {
                avatar = AvatarHelper.GetAvatarUrl(member.Avatar, AvatarSize.X80, FromType.User);
            }

            var toastContent = new ToastContent()
            {
                DisplayTimestamp = apiMsg.CreatedAt,
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = member.DisplayName,
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                Text = MessageHelper.GetContent(apiMsg)
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = avatar,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                }
            };

            if (apiMsg.Body.At != null && apiMsg.Body.At.Count == 1)
            {
                toastContent.Actions = new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastTextBox("msg")
                        {
                            PlaceholderContent = "请输入消息快速回复"
                        }
                    },
                    Buttons =
                    {
                        new ToastButton("Send", $"action=reply&toType={apiMsg.To.Type}&to={apiMsg.To.Id}&from={apiMsg.Body.At.First()}")
                        {
                            ActivationType = ToastActivationType.Foreground,
                            ImageUri = "Assets/Images/Icons/send.png",
                            TextBoxId = "msg"
                        }
                    }
                };
                toastContent.Launch = "action=launch";
            }

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        private void ContentFrameNavigate(string app)
        {
            switch (app)
            {
                case "message":
                    _contentFrame.Navigate(typeof(MasterPage), this);
                    break;
                //case "mission":
                //    ContentFrame.Navigate(typeof(MissionPage));
                //    break;
                default:
                    _contentFrame.Navigate(typeof(WaitForDevelopmentPage));
                    break;
            }
        }

        public async Task SendMessageAsync(Domain.SocketMessageConverter.SocketMessageType msgType, object data)
        {
            string msg = SocketMessageConverter.Process(msgType, data);
            using (var dataWriter = new DataWriter(_socket.OutputStream))
            {
                dataWriter.WriteString(msg);
                await dataWriter.StoreAsync();
                dataWriter.DetachStream();
            }
        }

        public void ShowNotification(string text, NotificationLevel level, int duration = 0)
        {
            if (level == NotificationLevel.Default)
            {
                _inAppNotification.BorderBrush = Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
            }
            else
            {
                string key = level.ToString() + "Brush";
                _inAppNotification.BorderBrush = Application.Current.Resources[key] as SolidColorBrush;
            }
            _inAppNotification.Show(text, duration);
        }

        public void Dispose()
        {
            _socket.MessageReceived -= Socket_MessageReceived;
            _socket.Dispose();
        }
    }
}
