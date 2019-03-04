using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message.Detail.Content.Pin;

namespace Worktile.Views.Message.Detail.Content.Pin
{
    public sealed partial class ChannelPinnedPage : Page, INotifyPropertyChanged
    {
        public ChannelPinnedPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ChannelPinnedViewModel _viewModel;
        private ChannelPinnedViewModel ViewModel
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
            var session = e.Parameter as ChannelSession;
            ViewModel = new ChannelPinnedViewModel(session);
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var msg = btn.DataContext as Models.Message.Message;
            await ViewModel.UnPinAsync(msg);
        }
    }
}
