using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextWorkAddonsKeyGroups;
using Worktile.ApiModels.ApiMissionVnextWorkAddon;
using Worktile.Common;
using Worktile.WtRequestClient;
using Value = Worktile.ApiModels.ApiMissionVnextWorkAddon.Value;

namespace Worktile.Views.Mission.My
{
    public sealed partial class IndexPage : Page, INotifyPropertyChanged
    {
        public IndexPage()
        {
            InitializeComponent();
            TopNavItems = new ObservableCollection<TopNavItem>();
            Status = new List<StatusItem>
            {
                new StatusItem { Text = "活动任务", Value = "active" },
                new StatusItem { Text = "完成任务", Value = "completed" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Value _navItem;

        public ObservableCollection<TopNavItem> TopNavItems { get; }

        public List<StatusItem> Status { get; }

        private StatusItem _selectedStatus;
        public StatusItem SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SelectedStatus != value)
                {
                    _selectedStatus = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
                    FrameNavigate();
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
                    FrameNavigate();
                }
            }
        }

        private void FrameNavigate()
        {
            if (SelectedNav == null || SelectedStatus == null)
            {
                return;
            }
            if (SelectedNav.Key == "directed" && Status.IndexOf(SelectedStatus) == 0)
            {
                ContentFrame.Navigate(typeof(MyDirectActivePage));
            }
            else
            {
                string uri = SelectedNav.Key + "/" + SelectedStatus.Value;
                ContentFrame.Navigate(typeof(GenericPage), uri);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navItem = e.Parameter as Value;
            TopNavIcon = WtIconHelper.GetGlyph(_navItem.Icon);
            TopNavName = _navItem.Name;
            SelectedStatus = Status.First();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string uri = $"/api/mission-vnext/work-addons/{_navItem.Key}/groups";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAddonsKeyGroups>(uri);
            foreach (var item in data.Data.Value)
            {
                TopNavItems.Add(new TopNavItem
                {
                    Key = item.Id,
                    Name = item.Name
                });
            }
            SelectedNav = TopNavItems.First();
        }
    }

    public class StatusItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class TopNavItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
