using Windows.UI.Xaml.Controls;
using Worktile.ViewModels;

namespace Worktile.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
        }

        public MainViewModel ViewModel { get; }
    }
}
