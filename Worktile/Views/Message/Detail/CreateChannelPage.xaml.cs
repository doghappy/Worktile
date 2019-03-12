using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Member;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message.Detail;
using Worktile.Common;
using Worktile.ViewModels;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class CreateChannelPage : Page
    {
        public CreateChannelPage()
        {
            InitializeComponent();
            ViewModel = new CreateChannelViewModel();
        }

        CreateChannelViewModel ViewModel { get; }

        MainViewModel _mainViewModel;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ChannelNameTextBox.Focus(FocusState.Programmatic);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _mainViewModel = e.Parameter as MainViewModel;
        }

        private void ClosePage()
        {
            var masterPage = Frame.GetParent<MasterPage>();
            var frame = masterPage.GetChild<Frame>("ContentFrame");
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
            else
            {
                frame.Navigate(typeof(TransparentPage));
            }
            _mainViewModel.NavigationView.IsBackEnabled = false;
            _mainViewModel.NavigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            ClosePage();
        }

        private async void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowError = false;
            if (string.IsNullOrWhiteSpace(ViewModel.ChannelName))
            {
                ViewModel.ErrorText = "群组名称不能为空";
                ViewModel.ShowError = true;
                ChannelNameTextBox.Focus(FocusState.Programmatic);
            }
            else
            {
                var res = await ViewModel.CreateAsync();
                if (res)
                {
                    ClosePage();
                }
                else
                {
                    ViewModel.ErrorText = "创建失败";
                    ViewModel.ShowError = true;
                }
            }
        }
    }
}
