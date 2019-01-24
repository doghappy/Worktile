using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Worktile.WtRequestClient;
using Worktile.ApiModels.ApiMissionVnextWorkAnalyticInsightGroups;
using System.Collections.ObjectModel;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class IndexPage : Page, INotifyPropertyChanged
    {
        public IndexPage()
        {
            InitializeComponent();
            TopNavItems = new ObservableCollection<TopNavItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ApiModels.ApiMissionVnextWorkAddon.Value _navItem;

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
            _navItem = e.Parameter as ApiModels.ApiMissionVnextWorkAddon.Value;
            TopNavIcon = WtIconHelper.GetGlyph(_navItem.Icon);
            TopNavName = _navItem.Name;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestApiMissionVnextWorkAnalyticInsightGroups();
            IsActive = false;
        }

        private async Task RequestApiMissionVnextWorkAnalyticInsightGroups()
        {
            string uri = "/api/mission-vnext/work/analytic-insight/groups";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAnalyticInsightGroups>(uri);
            foreach (var item in data.Data.Value)
            {
                var tni = new TopNavItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    SubItems = new List<TopNavItem>()
                };
                foreach (var ins in item.Insights)
                {
                    tni.SubItems.Add(new TopNavItem
                    {
                        Id = ins.Id,
                        Name = ins.Name,
                        Key = ins.Key
                    });
                }
                TopNavItems.Add(tni);
            }
            SelectedNav = TopNavItems.First();
            SelectedSubNav = SelectedNav.SubItems.First();
        }
    }

    public class TopNavItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public List<TopNavItem> SubItems { get; set; }
    }
}
