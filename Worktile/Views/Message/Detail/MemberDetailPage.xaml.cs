using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Operators.Message.Detail;
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
            MemberDetailOperator.ContentFrame = ContentFrame;
            var session = e.Parameter as MemberSession;
            ViewModel = new MemberDetailViewModel(session);
        }

        private async void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.IsPaneOpen = true;
            await ViewModel.LoadMemberInfoAsync();
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StarSessionAsync(ViewModel.Session);
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UnStarSessionAsync(ViewModel.Session);
        }
    }
}
