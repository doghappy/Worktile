using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Member;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message.Detail;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class AddMemberPage : Page
    {
        public AddMemberPage()
        {
            InitializeComponent();
        }

        AddMemberViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var session = e.Parameter as ChannelSession;
            ViewModel = new AddMemberViewModel(session);
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var member = btn.DataContext as Member;
            await ViewModel.AddMemberAsync(member);
        }
    }
}
