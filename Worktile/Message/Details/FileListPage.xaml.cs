using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Models;

namespace Worktile.Message.Details
{
    public sealed partial class FileListPage : Page
    {
        public FileListPage()
        {
            InitializeComponent();
            ViewModel = new FileListViewModel();
        }

        public FileListViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
        }
    }
}
