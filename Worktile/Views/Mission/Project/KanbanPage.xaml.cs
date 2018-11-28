using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.ViewModels.Mission.Project;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class KanbanPage : Page
    {
        public KanbanPage()
        {
            InitializeComponent();
            ViewModel = new KanbanViewModel
            {
                PropertyDefaultForeground = Resources["SystemControlForegroundBaseMediumBrush"] as SolidColorBrush,
                PropertyDefaultBackground = Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush
            };
        }

        public KanbanViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            ViewModel.AddonId = parameters.AddonId;
            ViewModel.ViewId = parameters.ViewId;
            ViewModel.TaskIdentifierPrefix = parameters.TaskIdentifierPrefix;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsActive = true;
            await ViewModel.RequestContentAsync();
            ViewModel.IsActive = false;
        }

        private async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await KanbanPageHelper.KanbanGridAdaptiveAsync(MyGrid);
        }
    }
}
