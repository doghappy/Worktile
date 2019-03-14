using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common.Communication;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message;
using Worktile.ViewModels.Message.Detail.Content;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class AssistantMessagePage : Page, INotifyPropertyChanged
    {
        public AssistantMessagePage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ToUnReadMsgPageParam _param;

        private AssistantMessageViewModel _viewModel;
        private AssistantMessageViewModel ViewModel
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _param = e.Parameter as ToUnReadMsgPageParam;
            var session = _param.Session as MemberSession;
            ViewModel = new AssistantMessageViewModel(session, _param.Nav);
            WtSocket.OnMessageReceived += ViewModel.OnMessageReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WtSocket.OnMessageReceived -= ViewModel.OnMessageReceived;
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (ViewModel.HasMore.HasValue && ViewModel.HasMore.Value && !ViewModel.IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await ViewModel.LoadMessagesAsync();
            }
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            await ViewModel.PinAsync(msg);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            await ViewModel.UnPinAsync(msg);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
            await ViewModel.ClearUnReadAsync();
        }
    }
}
