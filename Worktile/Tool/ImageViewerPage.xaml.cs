using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Tool.Models;

namespace Worktile.Tool
{
    public sealed partial class ImageViewerPage : Page
    {
        public ImageViewerPage()
        {
            InitializeComponent();
        }

        public ImageViewerData Data { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Data = e.Parameter as ImageViewerData;
        }
    }
}
