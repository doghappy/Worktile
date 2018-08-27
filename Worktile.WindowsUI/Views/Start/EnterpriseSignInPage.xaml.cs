using System;
using System.ComponentModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.WindowsUI.ViewModels.Start;

namespace Worktile.WindowsUI.Views.Start
{
    public sealed partial class EnterpriseSignInPage : Page, INotifyPropertyChanged
    {
        public EnterpriseSignInPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private EnterpriseSignInViewModel viewModel;
        public EnterpriseSignInViewModel ViewModel
        {
            get => viewModel;
            set
            {
                viewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new EnterpriseSignInViewModel();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://worktile.com/signup"));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.VerifyDomainAsync();
        }
    }
}
