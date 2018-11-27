using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextProjectsDetail;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class IndexPage : Page, INotifyPropertyChanged
    {
        public IndexPage()
        {
            InitializeComponent();
            TopNavItems = new ObservableCollection<TopNavItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _navId;
        private string _taskIdentifierPrefix;

        public ObservableCollection<TopNavItem> TopNavItems { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
            }
        }

        private string _topNavIcon;
        public string TopNavIcon
        {
            get => _topNavIcon;
            set
            {
                _topNavIcon = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavIcon)));
            }
        }

        private string _topNavName;
        public string TopNavName
        {
            get => _topNavName;
            set
            {
                _topNavName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavName)));
            }
        }

        private string _topNavColor;
        public string TopNavColor
        {
            get => _topNavColor;
            set
            {
                _topNavColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavColor)));
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
                    SelectedNavView = value.Views.FirstOrDefault();
                }
            }
        }

        private TopNavItemView _selectedNavView;
        public TopNavItemView SelectedNavView
        {
            get => _selectedNavView;
            set
            {
                if (SelectedNavView != value)
                {
                    _selectedNavView = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNavView)));
                    if (SelectedNav.TargetPage != null)
                    {
                        ContentFrame.Navigate(SelectedNav.TargetPage, new
                        {
                            AddonId = SelectedNav.Id,
                            ViewId = value.Id,
                            TaskIdentifierPrefix = _taskIdentifierPrefix
                        });
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var navItem = e.Parameter as NavItem;
            TopNavIcon = navItem.Glyph;
            TopNavName = navItem.Name;
            TopNavColor = navItem.Color;
            _navId = navItem.Id;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestApiMissionVnextProjectsDetail();
            IsActive = false;
        }

        private async Task RequestApiMissionVnextProjectsDetail()
        {
            string uri = $"/api/mission-vnext/projects/{_navId}?members=true&addons=true";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextProjectsDetail>(uri);
            _taskIdentifierPrefix = data.Data.Value.TaskIdentifierPrefix;
            foreach (var item in data.Data.References.Addons)
            {
                TopNavItems.Add(new TopNavItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Key = item.Key,
                    TargetPage = GetPage(item.Key),
                    Views = item.Views.Select(v => new TopNavItemView
                    {
                        Id = v.Id,
                        Name = v.Name
                    })
                    .ToList()
                });
            }
            SelectedNav = TopNavItems.First();
        }

        private Type GetPage(string key)
        {
            Type page = null;
            switch (key)
            {
                case "kanban": page = typeof(KanbanPage); break;
                case "table": page = typeof(TablePage); break;
                case "insight": page = typeof(InsightPage); break;
                case "calendar": page = typeof(CalendarPage); break;
                case "iteration": page = typeof(IterationPage); break;
                case "list": page = typeof(ListPage); break;
                case "timeline": page = typeof(TimelinePage); break;
                case "workload": page = typeof(WorkloadPage); break;
            }
            return page;
        }
    }

    public class TopNavItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public Type TargetPage { get; set; }
        public List<TopNavItemView> Views { get; set; }
    }

    public class TopNavItemView
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
