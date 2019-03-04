using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Worktile.ViewModels.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message;
using Worktile.ViewModels.Message.Detail.Content;
using Worktile.Models.Message.Session;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class ChannelMessagePage : Page, INotifyPropertyChanged
    {
        public ChannelMessagePage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ChannelMessageViewModel _viewModel;
        private ChannelMessageViewModel ViewModel
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

        private ToUnReadMsgPageParam _param;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _param = e.Parameter as ToUnReadMsgPageParam;
            var session = _param.Session as ChannelSession;
            ViewModel = new ChannelMessageViewModel(session, _param.MainViewModel);
            _param.MainViewModel.OnMessageReceived += ViewModel.OnMessageReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _param.MainViewModel.OnMessageReceived -= ViewModel.OnMessageReceived;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
            if (MsgTextBox != null)
                MsgTextBox.Focus(FocusState.Programmatic);
            await ViewModel.ClearUnReadAsync();
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (ViewModel.HasMore.HasValue && ViewModel.HasMore.Value && !ViewModel.IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await ViewModel.LoadMessagesAsync();
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
                var result = await ViewModel.SendMessageAsync(msg);
                if (result)
                {
                    MsgTextBox.Text = string.Empty;
                }
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
            var msg = flyoutItem.DataContext as Models.Message.Message;
            await ViewModel.PinMessageAsync(msg);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            await ViewModel.UnPinMessageAsync(msg);
        }
    }
}
