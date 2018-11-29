using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.ViewModels.Mission.AnalyticInsight;
using Worktile.Models.Mission.AnalyticInsight;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class IndexPage : Page, INotifyPropertyChanged
    {
        public IndexPage()
        {
            InitializeComponent();
            ViewModel = new IndexViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IndexViewModel ViewModel { get; }

        public ObservableCollection<TopNavItem> TopNavItems { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private string _topNavIcon;
        public string TopNavIcon
        {
            get => _topNavIcon;
            set
            {
                if (_topNavIcon != value)
                {
                    _topNavIcon = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavIcon)));
                }
            }
        }

        private string _topNavName;
        public string TopNavName
        {
            get => _topNavName;
            set
            {
                if (_topNavName != value)
                {
                    _topNavName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavName)));
                }
            }
        }

        private TopNavItem _selectedNav;
        public TopNavItem SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (SelectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                    SelectedSubNav = value.SubItems.FirstOrDefault();
                }
            }
        }

        private TopNavItem _selectedSubNav;
        public TopNavItem SelectedSubNav
        {
            get => _selectedSubNav;
            set
            {
                if (SelectedSubNav != value)
                {
                    _selectedSubNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSubNav)));
                    if (value == null)
                    {
                        ContentFrame.Navigate(typeof(EmptyPage), "请先设置报表");
                    }
                    else
                    {
                        var param = new { NavId = SelectedNav.Id, SubNavId = SelectedSubNav.Id };
                        switch (value.Key)
                        {
                            case "project-progress":
                                ContentFrame.Navigate(typeof(ProjectProgressPage), param);
                                break;
                            case "project-delay":
                                ContentFrame.Navigate(typeof(ProjectDelayPage), param);
                                break;
                            case "member-progress":
                                ContentFrame.Navigate(typeof(MemberProgressPage), param);
                                break;
                            case "member-delay":
                                ContentFrame.Navigate(typeof(MemberDelayPage), param);
                                break;
                        }
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navItem = e.Parameter as ApiModels.ApiMissionVnextWorkAddon.Value;
            ViewModel.TopNavIcon = WtIconHelper.GetGlyph(navItem.Icon);
            ViewModel.TopNavName = navItem.Name;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsActive = true;
            await ViewModel.RequestApiDataAsync();
            SelectedNav = ViewModel.TopNavItems.First();
            SelectedSubNav = SelectedNav.SubItems.First();
            ViewModel.IsActive = false;
        }
    }
}
