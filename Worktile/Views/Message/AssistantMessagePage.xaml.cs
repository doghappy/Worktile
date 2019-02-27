using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.ViewModels.Message;

namespace Worktile.Views.Message
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
            ViewModel = new AssistantMessageViewModel(_param.Session, _param.Nav);
            _param.MainViewModel.OnMessageReceived += ViewModel.OnMessageReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _param.MainViewModel.OnMessageReceived -= ViewModel.OnMessageReceived;
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
            var msg = flyoutItem.DataContext as ViewMessage;
            await ViewModel.PinMessageAsync(msg);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as ViewMessage;
            await ViewModel.UnPinMessageAsync(msg);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadMessagesAsync();
            await ViewModel.ClearUnReadAsync(_param.MainViewModel);
        }
    }
}
