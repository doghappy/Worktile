using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.NavigationParam;
using Worktile.ViewModels.Message;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class ChannelDetailPage : Page, INotifyPropertyChanged
    {
        public ChannelDetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ToMessageDetailPageParam _navParam;

        private ChannelDetailViewModel _viewModel;
        private ChannelDetailViewModel ViewModel
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
            _navParam = e.Parameter as ToMessageDetailPageParam;
            ViewModel = new ChannelDetailViewModel(_navParam.Session, ContentFrame, _navParam.MainViewModel);
        }

        private async void ContactInfo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.IsPaneOpen = true;
            //await ViewModel.LoadMemberInfoAsync();
        }
    }
}
