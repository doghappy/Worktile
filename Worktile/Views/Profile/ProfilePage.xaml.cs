using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common.WtRequestClient;
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
            ApplicationData.Current.LocalSettings.Values.Remove("AuthCookie");
            WtHttpClient.Reset();
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(PasswordSignInPage));
        }
    }
}
