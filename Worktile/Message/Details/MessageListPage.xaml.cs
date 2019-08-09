using System;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models;
using Worktile.Tool;
using Worktile.Tool.Models;
using WtMessage = Worktile.Message.Models;
using System.Linq;
using Worktile.Main;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.Storage;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Web.Http;

namespace Worktile.Message.Details
{
    public sealed partial class MessageListPage : Page, INotifyPropertyChanged
    {
        public MessageListPage()
        {
            InitializeComponent();
            Messages = new ObservableCollection<WtMessage.Message>();
            HasMore = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WtMessage.Message> Messages { get; }

        public bool HasMore { get; private set; }

        public Session Session { get; private set; }

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

        string _latestId = null;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMessagesAsync();
            WtSocketClient.OnMessageReceived += OnMessageReceived;
            //MainViewModel.UnreadMessageCount -= ViewModel.Session.UnRead;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Session = e.Parameter as Session;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WtSocketClient.OnMessageReceived -= OnMessageReceived;
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
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
            await UploadFileAsync(files);
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

        private async void OnMessageReceived(JObject obj)
        {
            var msg = obj.ToObject<WtMessage.Message>();
            if (msg.To.Id == Session.Id)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    msg.CompleteMessageFrom();
                    Messages.Add(msg);
                });
                //await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Messages.Add(msg)));
            }
        }

        private async Task SendMessageAsync()
        {
            string to = Session.Id;
            WtMessage.ToType toType = WtMessage.ToType.Channel;
            if (Session.Type == SessionType.Session)
            {
                toType = WtMessage.ToType.Session;
            }
            string message = MsgTextBox.Text.Trim();
            if (message != string.Empty)
            {
                await WtSocketClient.SendMessageAsync(to, toType, message, WtMessage.MessageType.Text);
            }
            MsgTextBox.Text = string.Empty;
        }

        long _ticks = 0;

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            long ticks = DateTime.Now.Ticks;
            if (HasMore
                && scrollViewer.VerticalOffset <= 10
                && (_ticks == 0 || new TimeSpan(ticks - _ticks).TotalSeconds > .5))
            {
                _ticks = ticks;
                await LoadMessagesAsync();
            }
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as WtMessage.Message;
            string idType = Session.Type == SessionType.Session ? "session_id" : "channel_id";
            var req = new
            {
                type = 1,
                message_id = msg.Id,
                worktile = Session.Id
            };
            string json = JsonConvert.SerializeObject(req);
            json = json.Replace("worktile", idType);
            var obj = await WtHttpClient.PostAsync("api/pinneds", json);
            if (obj.Value<int>("code") == 200)
            {
                msg.IsPinned = true;
            }
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as WtMessage.Message;
            string idType = Session.Type == SessionType.Session ? "session_id" : "channel_id";
            string url = $"api/messages/{msg.Id}/unpinned?{idType}={Session.Id}";
            var obj = await WtHttpClient.DeleteAsync(url);
            if (obj.Value<int>("code") == 200 && obj.Value<bool>("data"))
            {
                msg.IsPinned = false;
            }
        }

        private async void MessageControl_OnImageMessageClick(WtMessage.Message message)
        {
            var data = new ImageViewerData
            {
                SelectedItem = UtilityTool.GetS3FileUrl(message.Body.Attachment.Id),
                ItemSource = Messages
                    .Where(m => m.Type == WtMessage.MessageType.Image)
                    .Select(m => UtilityTool.GetS3FileUrl(m.Body.Attachment.Id))
                    .ToList()
            };

            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(ImageViewerPage), data);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async Task LoadMessagesAsync()
        {
            if (HasMore)
            {
                IsActive = true;
                string url = $"api/messages?ref_id={Session.Id}&ref_type={Session.RefType}&latest_id={_latestId}&size=20";
                var obj = await WtHttpClient.GetAsync(url);
                if (_latestId != null)
                {
                    HasMore = obj["data"].Value<bool>("more");
                }
                _latestId = obj["data"].Value<string>("latest_id");
                var msgs = obj["data"]["messages"].Children<JObject>().OrderByDescending(g => g.Value<double>("created_at"));
                foreach (var item in msgs)
                {
                    Messages.Insert(0, item.ToObject<WtMessage.Message>());
                }
                IsActive = false;
            }
        }

        private async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            if (files.Any())
            {
                string refType = Session.Type == SessionType.Session ? "2" : "1";
                string url = $"{MainViewModel.Box.BaseUrl}/entities/upload?team_id={MainViewModel.TeamId}&ref_id={Session.Id}&ref_type={refType}";
                foreach (var file in files)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
                    using (var stream = await file.OpenReadAsync())
                    {
                        string fileName = file.DisplayName + file.FileType;
                        var content = new HttpMultipartFormDataContent
                        {
                            { new HttpStringContent(fileName), "name" },
                            { new HttpStreamContent(stream), "file", fileName }
                        };
                        await WtHttpClient.PostAsync(url, content);
                    }
                }
            }
        }
    }
}
