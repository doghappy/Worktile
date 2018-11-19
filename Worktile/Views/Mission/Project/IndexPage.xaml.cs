using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextProjectsDetail;
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
            foreach (var item in data.Data.References.Addons)
            {
                TopNavItems.Add(new TopNavItem
                {
                    Id = item.Id,
                    Name = item.Name,
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
    }

    public class TopNavItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TopNavItemView> Views { get; set; }
    }

    public class TopNavItemView
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
