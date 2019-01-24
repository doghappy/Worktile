using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ViewModels.IM;

namespace Worktile.WtControls.IM
{
    public sealed partial class ChatPivot : UserControl
    {
        public ChatPivot()
        {
            InitializeComponent();
        }

        public MessageViewModel ViewModel
        {
            get { return (MessageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MessageViewModel), typeof(ChatPivot), new PropertyMetadata(null));

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (ViewModel.SelectedNav.HasMore.HasValue && ViewModel.SelectedNav.HasMore.Value
                && !ViewModel.IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await ViewModel.LoadMessagesAsync();
            }
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ViewModel.SelectedNav.HasMore.HasValue)
            {
                //当HasMore没有值时，表示没有载入过消息，需要请求接口。
                await ViewModel.LoadMessagesAsync();
            }
        }
    }
}
