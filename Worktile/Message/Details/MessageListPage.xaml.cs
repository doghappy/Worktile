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

namespace Worktile.Message.Details
{
    public sealed partial class MessageListPage : Page
    {
        public MessageListPage()
        {
            InitializeComponent();
            ViewModel = new MessageListViewModel();
        }

        public MessageListViewModel ViewModel { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
            WtSocketClient.OnMessageReceived += ViewModel.OnMessageReceived;
            MainViewModel.UnreadMessageCount -= ViewModel.Session.UnRead;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WtSocketClient.OnMessageReceived -= ViewModel.OnMessageReceived;
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
            await ViewModel.UploadFileAsync(files);
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

        private async Task SendMessageAsync()
        {
            await ViewModel.SendMessageAsync(MsgTextBox.Text);
            MsgTextBox.Text = string.Empty;
        }

        long _ticks = 0;

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            long ticks = DateTime.Now.Ticks;
            if (ViewModel.HasMore
                && scrollViewer.VerticalOffset <= 10
                && (_ticks == 0 || new TimeSpan(ticks - _ticks).TotalSeconds > .5))
            {
                _ticks = ticks;
                await ViewModel.LoadMessagesAsync();
            }
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as WtMessage.Message;
            await ViewModel.PinAsync(msg);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as WtMessage.Message;
            await ViewModel.UnPinAsync(msg);
        }

        private async void MessageControl_OnImageMessageClick(WtMessage.Message message)
        {
            var data = new ImageViewerData
            {
                SelectedItem = UtilityTool.GetS3FileUrl(message.Body.Attachment.Id),
                ItemSource = ViewModel.Messages
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
    }
}
