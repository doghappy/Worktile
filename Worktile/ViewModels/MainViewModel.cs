using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
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
using Worktile.ApiModels.ApiUserMe;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Domain.MessageContentReader;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Domain.SocketMessageConverter.Converters;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Views;
using Worktile.Views.Message;
using Worktile.ApiModels;
using Worktile.Models.Team;
using Worktile.Common.Extensions;

namespace Worktile.ViewModels
{
    public class MainViewModel : ViewModel, INotifyPropertyChanged, IDisposable
    {
        public MainViewModel()
        {
            Apps = new ObservableCollection<WtApp>();
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MessageWebSocket _socket;
        public event Action<Models.Message.Message> OnMessageReceived;
        public event Action<Models.Message.Feed> OnFeedReceived;

        private ThreadPoolTimer _timer;
        private ThreadPoolTimer _reconnectTimer;
        public bool _showConnSuccessNoti;

        public string SocketId { get; private set; }
        public Frame ContentFrame { get; set; }
        public InAppNotification InAppNotification { get; set; }
        public CoreDispatcher Dispatcher { get; set; }

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

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    var helper = sender as NetworkHelper;
                    if (helper.ConnectionInformation.IsInternetAvailable)
                    {
                        ShowNotification("网络已恢复，正在重新连接……", NotificationLevel.Warning, 4000);
                    }
                    else
                    {
                        ShowNotification("网络已断开，正在重新连接……", NotificationLevel.Danger, 4000);
                        ReConnect();
                    }
                });
            });
        }

        public async Task RequestApiUserMeAsync()
        {
            var me = await WtHttpClient.GetAsync<ApiUserMe>("/api/user/me");
            DataSource.ApiUserMeData = me.Data;
            DisplayName = DataSource.ApiUserMeData.Me.DisplayName;

            string bgImg = DataSource.ApiUserMeData.Me.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BgImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = DataSource.ApiUserMeData.Config.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                var buffer = await WtHttpClient.GetByteBufferAsync(imgUriString);

                BgImage = await ImageHelper.GetImageAsync(buffer);
            }

            await ConnectSocketAsync();
        }

        public async Task RequestApiTeamAsync()
        {
            var data = await WtHttpClient.GetAsync<ApiDataResponse<Team>>("/api/team");
            DataSource.Team = data.Data;
            Apps.Add(new WtApp
            {
                Name = "message",
                Icon = WtIconHelper.GetAppIcon("message"),
                DisplayName = "消息"
            });
            DataSource.Team.Apps.ForEach(app =>
            {
                app.Icon = WtIconHelper.GetAppIcon(app.Name);
                Apps.Add(app);
            });
            SelectedApp = Apps.First();
            Logo = new BitmapImage(new Uri(DataSource.ApiUserMeData.Config.Box.LogoUrl + DataSource.Team.Logo));

            DataSource.Team.Members.ForEach(m => m.ForShowAvatar(AvatarSize.X80));
        }

        private async Task ConnectSocketAsync()
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    var aaa = System.Threading.Thread.CurrentThread.ManagedThreadId;
                    try
                    {
                        _socket = new MessageWebSocket();
                        _socket.Control.MessageType = Windows.Networking.Sockets.SocketMessageType.Utf8;
                        _socket.MessageReceived += Socket_MessageReceived;
                        _socket.Closed += Socket_Closed;

                        Uri uri = new Uri($"wss://im.worktile.com/socket.io/?token={DataSource.ApiUserMeData.Me.ImToken}&uid={DataSource.ApiUserMeData.Me.Uid}&client=web&EIO=3&transport=websocket");
                        try
                        {
                            await _socket.ConnectAsync(uri);
                            if (_showConnSuccessNoti)
                            {
                                ShowNotification("网络已恢复，已经重新连接到消息服务了。", NotificationLevel.Success, 4000);
                            }
                        }
                        catch (Exception e)
                        {
                            ShowNotification("与消息服务器断开连接，正在重新连接……", NotificationLevel.Danger, 4000);
                            if (_reconnectTimer == null)
                            {
                                ReConnect();
                            }
                            throw e;
                        }

                        using (var dataWriter = new DataWriter(_socket.OutputStream))
                        {
                            string msg = $"40/message?token={DataSource.ApiUserMeData.Me.ImToken}&uid={DataSource.ApiUserMeData.Me.Uid}&client=web";
                            dataWriter.WriteString(msg);
                            await dataWriter.StoreAsync();
                            dataWriter.DetachStream();
                        }
                        KeepConnection();
                        if (_reconnectTimer != null)
                        {
                            _reconnectTimer.Cancel();
                            _reconnectTimer = null;
                        }
                    }
                    catch { }
                });
            });
        }

        private void ReConnect()
        {
            _timer.Cancel();
            _timer = null;
            _socket = null;
            if (_reconnectTimer == null)
            {
                _showConnSuccessNoti = true;
                _reconnectTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await ConnectSocketAsync();
                    });

                }, TimeSpan.FromSeconds(5));
            }
        }

        private void Socket_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            ReConnect();
        }

        private void Socket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
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
            catch { }
        }

        /// <summary>
        /// 心跳机制，防止服务端断开连接。
        /// </summary>
        private void KeepConnection()
        {
            if (_timer == null)
            {
                _timer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        using (var dataWriter = new DataWriter(_socket.OutputStream))
                        {
                            dataWriter.WriteString("2");
                            try
                            {
                                await dataWriter.StoreAsync();
                                dataWriter.DetachStream();
                            }
                            catch { }
                        }
                    });

                }, TimeSpan.FromSeconds(30));
            }
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
                                Text = MessageContentReader.ReadSummary(apiMsg)
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
                    ContentFrame.Navigate(typeof(MasterPage), this);
                    break;
                //case "mission":
                //    ContentFrame.Navigate(typeof(MissionPage));
                //    break;
                default:
                    ContentFrame.Navigate(typeof(WaitForDevelopmentPage));
                    break;
            }
        }

        public async Task<bool> SendMessageAsync(Domain.SocketMessageConverter.SocketMessageType msgType, object data)
        {
            if (_socket != null)
            {
                string msg = SocketMessageConverter.Process(msgType, data);
                using (var dataWriter = new DataWriter(_socket.OutputStream))
                {
                    dataWriter.WriteString(msg);
                    try
                    {
                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                        return true;
                    }
                    catch { }
                }
            }
            return false;
        }

        public void ShowNotification(string text, NotificationLevel level, int duration = 0)
        {
            if (level == NotificationLevel.Default)
            {
                InAppNotification.BorderBrush = Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
            }
            else
            {
                string key = level.ToString() + "Brush";
                InAppNotification.BorderBrush = Application.Current.Resources[key] as SolidColorBrush;
            }
            InAppNotification.Show(text, duration);
        }

        public void Dispose()
        {
            _timer?.Cancel();
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }
    }
}
