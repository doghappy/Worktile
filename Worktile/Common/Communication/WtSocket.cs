using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Worktile.Domain.MessageContentReader;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Domain.SocketMessageConverter.Converters;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Views;
using Worktile.Views.Message;

namespace Worktile.Common.Communication
{
    static class WtSocket
    {
        private static MessageWebSocket _socket;
        public static bool _showConnSuccessNoti;
        private static ThreadPoolTimer _timer;
        private static ThreadPoolTimer _reconnectTimer;

        public static event Action<Models.Message.Message> OnMessageReceived;
        public static event Action<Models.Message.Feed> OnFeedReceived;

        public static string SocketId { get; private set; }

        public static async Task ConnectSocketAsync()
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
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
                                LightMainPage.ShowNotification("网络已恢复，已经重新连接到消息服务了。", NotificationLevel.Success, 4000);
                            }
                        }
                        catch (Exception e)
                        {
                            LightMainPage.ShowNotification("与消息服务器断开连接，正在重新连接……", NotificationLevel.Danger, 4000);
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

        public static void ReConnect()
        {
            _timer.Cancel();
            _timer = null;
            _socket = null;
            if (_reconnectTimer == null)
            {
                _showConnSuccessNoti = true;
                _reconnectTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        await ConnectSocketAsync();
                    });

                }, TimeSpan.FromSeconds(5));
            }
        }

        private static void Socket_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            ReConnect();
        }

        public static void Socket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                using (DataReader dataReader = args.GetDataReader())
                {
                    dataReader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
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
        private static void KeepConnection()
        {
            if (_timer == null)
            {
                _timer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
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
        private static void MessageReceived(string msg)
        {
            var apiMsg = JsonConvert.DeserializeObject<Models.Message.Message>(msg);
            OnMessageReceived?.Invoke(apiMsg);
            if (apiMsg.From.Uid != DataSource.ApiUserMeData.Me.Uid)
            {
                App.UnreadBadge += 1;

                if (Window.Current.Content is LightMainPage mainPage)
                {
                    var masterPage = mainPage.GetChildren<MasterPage>().FirstOrDefault();
                    if (mainPage.WindowActivationState == CoreWindowActivationState.Deactivated || masterPage == null)
                    {
                        SendToast(apiMsg);
                    }
                }
            }
        }

        private static void SendToast(Models.Message.Message apiMsg)
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

        private static void FeedReceived(string msg)
        {
            var feed = JsonConvert.DeserializeObject<Models.Message.Feed>(msg);
            OnFeedReceived?.Invoke(feed);
        }

        public static async Task<bool> SendMessageAsync(Domain.SocketMessageConverter.SocketMessageType msgType, object data)
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

        public static void Dispose()
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
