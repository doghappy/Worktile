using Windows.UI.Xaml.Controls;
using Worktile.Main;

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
    }
}
