using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message;
using Worktile.ViewModels.Message.Detail;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class JoinChannelPage : Page
    {
        public JoinChannelPage()
        {
            InitializeComponent();
            ViewModel = new JoinChannelViewModel();
        }

        JoinChannelViewModel ViewModel { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.MasterViewModel = e.Parameter as MasterViewModel;
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var session = btn.DataContext as ChannelSession;
            var res = await ViewModel.ActiveAsync(session);
            if (res)
            {
                DisableGoBack();
            }
        }

        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var session = btn.DataContext as ChannelSession;
            var res = await ViewModel.JoinAsync(session);
            if (res)
            {
                DisableGoBack();
            }
        }

        private void DisableGoBack()
        {
            var mainNavView = this.GetParent<NavigationView>("MainNavView");
            mainNavView.IsBackEnabled = false;
            mainNavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
        }
    }
}
