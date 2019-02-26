using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModels.ApiTeam;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Models;
using Worktile.Common;
using Worktile.Views;
using Worktile.Common.WtRequestClient;
using Windows.UI.Xaml.Navigation;
using Worktile.Views.Message;
using Windows.Networking.Sockets;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Storage.Streams;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Domain.SocketMessageConverter.Converters;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Worktile.Enums;
using Windows.System.Threading;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media;

namespace Worktile
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            Apps = new ObservableCollection<WtApp>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Property
        public ObservableCollection<WtApp> Apps { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
            }
        }

        private BitmapImage _bgImage;
        public BitmapImage BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BgImage)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApp)));
                    if (value != null)
                    {
                        ContentFrameNavigate(value.Name);
                    }
                }
            }
        }
        #endregion

        #region Init
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            string cookie = ApplicationData.Current.LocalSettings.Values[SignInPage.AuthCookie]?.ToString();
            if (string.IsNullOrEmpty(cookie))
            {
                Frame.Navigate(typeof(SignInPage));
            }
            else
            {
                WtHttpClient.SetBaseAddress(DataSource.SubDomain);
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookie);
                Logo = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.scale-200.png"));
                await RequestApiUserMeAsync();
                await RequestApiTeamAsync();
            }
            IsActive = false;
            Window.Current.Activated += Window_Activated;
        }

        private CoreWindowActivationState _windowActivationState;

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            _windowActivationState = e.WindowActivationState;
        }

        private async Task RequestApiUserMeAsync()
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

        private async Task RequestApiTeamAsync()
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
        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _socket.MessageReceived -= Socket_MessageReceived;
            _socket.Dispose();
            Window.Current.Activated -= Window_Activated;
        }

        #region Socket
        public string SocketId { get; private set; }

        private MessageWebSocket _socket;
        public event Action<Models.Message.Message> OnMessageReceived;
        public event Action<Views.Message.Feed> OnFeedReceived;

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

        /// <summary>
        /// 心跳机制，防止服务端断开连接。
        /// </summary>
        private void KeepConnection()
        {
            var timer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
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
                if (_windowActivationState == CoreWindowActivationState.Deactivated)
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
            var feed = JsonConvert.DeserializeObject<Views.Message.Feed>(msg);
            OnFeedReceived?.Invoke(feed);
        }

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

        #endregion

        #region Navigate
        //private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        //{
        //    if (args.IsSettingsSelected)
        //    {
        //        ContentFrame.Navigate(typeof(TestPage));
        //    }
        //    else
        //    {
        //        var app = args.SelectedItem as WtApp;
        //        SelectedApp = app;
        //        ContentFrameNavigate(app.Name);
        //    }
        //}

        private void ContentFrameNavigate(string app)
        {
            switch (app)
            {
                case "message":
                    ContentFrame.Navigate(typeof(MessagePage), this);
                    break;
                //case "mission":
                //    ContentFrame.Navigate(typeof(MissionPage));
                //    break;
                default:
                    ContentFrame.Navigate(typeof(WaitForDevelopmentPage));
                    break;
            }
        }
        #endregion

        private void Me_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SelectedApp = null;
            ContentFrame.Navigate(typeof(WaitForDevelopmentPage));
        }

        private void Setting_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SelectedApp = null;
            ContentFrame.Navigate(typeof(TestPage));
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
    }

    public enum NotificationLevel
    {
        Default,
        Success,
        Warning,
        Danger
    }
}
