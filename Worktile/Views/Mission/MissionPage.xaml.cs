using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModel.ApiMissionVnextProjectNav;
using Worktile.ApiModel.ApiMissionVnextWorkAddon;
using Worktile.Services;
using Worktile.Views.Mission.My;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission
{
    public sealed partial class MissionPage : Page, INotifyPropertyChanged
    {
        public MissionPage()
        {
            InitializeComponent();
            WorkNavItems = new ObservableCollection<NavItem>();
            FavNavItems = new ObservableCollection<NavItem>();
            ProjectNavItems = new ObservableCollection<NavItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableCollection<NavItem> WorkNavItems { get; }

        private NavItem _selectedWorkNav;
        public NavItem SelectedWorkNav
        {
            get => _selectedWorkNav;
            set
            {
                if (SelectedWorkNav != value)
                {
                    _selectedFavNav = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFavNav)));
                    _selectedProjectNav = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProjectNav)));
                    _selectedWorkNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWorkNav)));
                    if (value != null)
                    {
                        var item = _workAddons.Single(w => w.Name == value.Name);
                        switch (item.Key)
                        {
                            case "my": ContentFrame.Navigate(typeof(IndexPage), item); break;
                        }
                    }
                }
            }
        }

        public ObservableCollection<NavItem> FavNavItems { get; }

        private NavItem _selectedFavNav;
        public NavItem SelectedFavNav
        {
            get => _selectedFavNav;
            set
            {
                if (SelectedFavNav != value)
                {
                    _selectedProjectNav = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProjectNav)));
                    _selectedWorkNav = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWorkNav)));
                    _selectedFavNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFavNav)));
                }
            }
        }

        public ObservableCollection<NavItem> ProjectNavItems { get; }

        private NavItem _selectedProjectNav;
        public NavItem SelectedProjectNav
        {
            get => _selectedProjectNav;
            set
            {
                if (SelectedProjectNav != value)
                {
                    _selectedFavNav = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFavNav)));
                    _selectedWorkNav = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWorkNav)));
                    _selectedProjectNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedProjectNav)));
                }
            }
        }

        private List<ApiModel.ApiMissionVnextWorkAddon.Value> _workAddons;

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestApiMissionVnextWorkAddon();
            await RequestApiMissionVnextProjectNav();
            SelectedWorkNav = WorkNavItems.First();
            IsActive = false;
        }

        private async Task RequestApiMissionVnextWorkAddon()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAddon>("/api/mission-vnext/work-addons");
            _workAddons = data.Data.Value;
            foreach (var item in data.Data.Value)
            {
                WorkNavItems.Add(new NavItem
                {
                    Name = item.Name,
                    Glyph = WtIconHelper.GetGlyph(item.Icon)
                });
            }
        }

        private async Task RequestApiMissionVnextProjectNav()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextProjectNav>("/api/mission-vnext/project-nav");
            foreach (var item in data.Data.ProjectNav.Favorites)
            {
                var project = data.Data.Projects.Single(p => p.Id == item);
                project.Color = WtColorHelper.GetNewColor(project.Color);
                FavNavItems.Add(new NavItem
                {
                    Name = project.Name,
                    Color = project.Color,
                    Glyph = project.Visibility == 1 ? "\ue70c" : "\ue667"
                });
            }

            foreach (var item in data.Data.ProjectNav.Items)
            {
                if (item.NavType == 1)
                {
                    var project = data.Data.Projects.Single(p => p.Id == item.NavId);
                    project.Color = WtColorHelper.GetNewColor(project.Color);
                    ProjectNavItems.Add(new NavItem
                    {
                        Name = project.Name,
                        Color = project.Color,
                        Glyph = project.Visibility == 1 ? "\ue70c" : "\ue667"
                    });
                }
                else if (item.NavType == 2)
                {
                    foreach (var subItem in item.Items)
                    {
                        var project = data.Data.Projects.Single(p => p.Id == subItem.NavId);
                        if (project != null)
                        {
                            project.Color = WtColorHelper.GetNewColor(project.Color);
                            ProjectNavItems.Add(new NavItem
                            {
                                Name = project.Name,
                                Color = project.Color,
                                Glyph = project.Visibility == 1 ? "\ue70c" : "\ue667"
                            });
                        }
                    }
                }
            }
        }
    }

    public class NavItem
    {
        public string Name { get; set; }
        public string Glyph { get; set; }
        public string Color { get; set; }
    }
}
