using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class MemberDetailPage : Page, INotifyPropertyChanged
    {
        public MemberDetailPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ToMessageDetailPageParam _navParam;

        private MemberDetailViewModel _viewModel;
        private MemberDetailViewModel ViewModel
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
            var session = _navParam.Session as MemberSession;
            ViewModel = new MemberDetailViewModel(session, ContentFrame, _navParam.MainViewModel);
        }

        private async void ContactInfo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.IsPaneOpen = true;
            await ViewModel.LoadMemberInfoAsync();
        }
    }
}
