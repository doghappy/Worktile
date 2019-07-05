using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Message.Details
{
    public sealed partial class PinListPage : Page
    {
        public PinListPage()
        {
            InitializeComponent();
            ViewModel = new PinListViewModel();
        }

        public PinListViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var msg = btn.DataContext as WtMessage.Message;
            await ViewModel.UnPinAsync(msg);
        }
    }
}
