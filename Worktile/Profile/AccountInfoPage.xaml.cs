using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models;
using Worktile.SignInOut;

namespace Worktile.Profile
{
    public sealed partial class AccountInfoPage : Page
    {
        public AccountInfoPage()
        {
            InitializeComponent();
            ViewModel = new AccountInfoViewModel();
        }

        public AccountInfoViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.User = e.Parameter as User;
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values.Remove("Domain");
            UtilityTool.RootFrame.Navigate(typeof(SignInPage));
        }
    }
}
