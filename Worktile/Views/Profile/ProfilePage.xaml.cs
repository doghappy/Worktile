using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ViewModels.Profile;
using Worktile.Views.SignIn;

namespace Worktile.Views.Profile
{
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            ViewModel = new ProfileViewModel();
        }

        ProfileViewModel ViewModel { get; }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            //ApplicationData.Current.LocalSettings.Values.Remove("Domain");
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(PasswordSignInPage));
        }
    }
}
