using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.WindowsUI.Models.Mission;
using Worktile.WindowsUI.ViewModels.Mission;

namespace Worktile.WindowsUI.Views.Mission
{
    public sealed partial class MyMissionPage : Page, INotifyPropertyChanged
    {
        public MyMissionPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MyMissionViewModel viewModel;
        public MyMissionViewModel ViewModel
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
            var workAddon = e.Parameter as WorkAddon;
            ViewModel = new MyMissionViewModel(workAddon);
        }
    }
}
