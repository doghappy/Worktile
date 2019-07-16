using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Worktile.Main;
using Worktile.Models;

namespace Worktile.Message
{
    public sealed partial class MessagePage : Page
    {
        public MessagePage()
        {
            InitializeComponent();
            ViewModel = new MessageViewModel();
        }

        public MessageViewModel ViewModel { get; }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Frame = ContentFrame;
            MainViewModel.MessageViewModel = ViewModel;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DeleteRightTappedSessionAsync();
        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            ViewModel.RightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as Session;
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.StarAsync();
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UnStarAsync();
        }

        private void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            //Type sourcePageType = typeof(CreateChannelPage);
            //EnableNavGoBack(sourcePageType);
        }

        private void JoinChannel_Click(object sender, RoutedEventArgs e)
        {
            //Type sourcePageType = typeof(JoinChannelPage);
            //EnableNavGoBack(sourcePageType);
        }

        //private async void AddMember_Click(object sender, RoutedEventArgs e)
        //{
        //    await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        //}

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            //Type sourcePageType = typeof(CreateChatPage);
            //EnableNavGoBack(sourcePageType);
        }
    }
}
