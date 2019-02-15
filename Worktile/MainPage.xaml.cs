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
using Worktile.Views.Mission;
using Worktile.WtRequestClient;
using Windows.UI.Xaml.Navigation;
using Worktile.Infrastructure;
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
using Worktile.Enums.IM;
using Windows.System.Threading;
using Windows.UI.Core;

namespace Worktile
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            AppItems = new ObservableCollection<WtApp>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Property
        public ObservableCollection<WtApp> AppItems { get; }

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
        }

        private async Task RequestApiUserMeAsync()
        {
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>("/api/user/me");
            DataSource.ApiUserMeConfig = me.Data.Config;
            DataSource.ApiUserMe = me.Data.Me;
            DisplayName = DataSource.ApiUserMe.DisplayName;

            await ConnectSocketAsync();

            string bgImg = DataSource.ApiUserMe.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BgImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = DataSource.ApiUserMeConfig.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                byte[] buffer = await client.GetByteArrayAsync(imgUriString);
                BgImage = await ImageHelper.GetImageFromBytesAsync(buffer);
            }
        }

        private async Task RequestApiTeamAsync()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>("/api/team");
            DataSource.Team = data.Data;
            AppItems.Add(DataSource.Apps.SingleOrDefault(a => a.Name == "message"));
            DataSource.Team.Apps.ForEach(app =>
            {
                var item = DataSource.Apps.Single(a => a.Name == app.Name);
                AppItems.Add(item);
            });
            //SelectedApp = AppItems.First();
            Logo = new BitmapImage(new Uri(DataSource.ApiUserMeConfig.Box.LogoUrl + DataSource.Team.Logo));
        }
        #endregion

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _socket.MessageReceived -= Socket_MessageReceived;
            _socket.Dispose();
        }

        #region Socket
        public static string SocketId { get; private set; }

        private static MessageWebSocket _socket;
        public static event Action<Models.IM.Message.Message> OnMessageReceived;

        private async Task ConnectSocketAsync()
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    _socket = new MessageWebSocket();
                    _socket.Control.MessageType = Windows.Networking.Sockets.SocketMessageType.Utf8;
                    _socket.MessageReceived += Socket_MessageReceived;
                    //MessageWebSocket.Closed += MessageWebSocket_Closed;

                    Uri uri = new Uri($"wss://im.worktile.com/socket.io/?token={DataSource.ApiUserMe.ImToken}&uid={DataSource.ApiUserMe.Uid}&client=web&EIO=3&transport=websocket");
                    await _socket.ConnectAsync(uri);

                    using (var dataWriter = new DataWriter(_socket.OutputStream))
                    {
                        string msg = $"40/message?token={DataSource.ApiUserMe.ImToken}&uid={DataSource.ApiUserMe.Uid}&client=web";
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
                await Dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
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
            var apiMsg = JsonConvert.DeserializeObject<Models.IM.Message.Message>(msg);
            OnMessageReceived?.Invoke(apiMsg);
            UnreadBadge += 1;
            SendToast(apiMsg);
        }

        private static int _unreadBadge;
        public static int UnreadBadge
        {
            get => _unreadBadge;
            set
            {
                if (_unreadBadge != value)
                {
                    _unreadBadge = value;
                    if (value > 0)
                        UpdateBadgeNumber(value);
                    else
                        BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
                }
            }
        }


        public static async Task SendMessageAsync(Domain.SocketMessageConverter.SocketMessageType msgType, object data)
        {
            string msg = SocketMessageConverter.Process(msgType, data);
            using (var dataWriter = new DataWriter(_socket.OutputStream))
            {
                dataWriter.WriteString(msg);
                await dataWriter.StoreAsync();
                dataWriter.DetachStream();
            }
        }

        public static void UpdateBadgeNumber(int number)
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

        private void SendToast(Models.IM.Message.Message apiMsg)
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
                                Text = ChatPage.GetContent(apiMsg)
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = avatar,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastTextBox("textBox")
                        {
                            PlaceholderContent = "请输入消息快速回复"
                        }
                    },
                    Buttons =
                    {
                        new ToastButton("Send", "action=reply&threadId=92187")
                        {
                            ActivationType = ToastActivationType.Background,
                            ImageUri = "Assets/Images/Icons/send.png",
                            TextBoxId = "textBox"
                            //InputId = "textBox"
                        }
                    }
                },
                //Launch = $"action=SendMessage&toType={apiMsg.To.Type}&to={apiMsg.To.Id}&from={apiMsg.From.Uid}"
                Launch = $"action=Message"
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        #endregion

        #region Navigate
        private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(TestPage));
            }
            else
            {
                var app = args.SelectedItem as WtApp;
                ContentFrameNavigate(app.Name);
            }
        }

        private void ContentFrameNavigate(string app)
        {
            switch (app)
            {
                case "message":
                    ContentFrame.Navigate(typeof(MessagePage));
                    break;
                case "mission":
                    ContentFrame.Navigate(typeof(MissionPage));
                    break;
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Nav.IsBackEnabled = ContentFrame.CanGoBack;
            if (e.NavigationMode == NavigationMode.Back)
            {
                int index = -1;
                switch (e.SourcePageType)
                {
                    case Type t when e.SourcePageType == typeof(MessagePage):
                        index = 0;
                        break;
                    case Type t when e.SourcePageType == typeof(MissionPage):
                        index = 2;
                        break;
                }
                if (index != -1)
                {
                    var item = Nav.MenuItems[index] as NavigationViewItem;
                    item.IsSelected = true;
                }
            }
        }

        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
        #endregion
    }
}
