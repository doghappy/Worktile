using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Models.Mission;
using Worktile.WindowsUI.Models.Results;
using Worktile.WindowsUI.Views.Mission;

namespace Worktile.WindowsUI.ViewModels.Project
{
    public class HomeViewModel : ViewModel, INotifyPropertyChanged
    {
        public HomeViewModel()
        {
            WorkAddons = new ObservableCollection<WorkAddon>();
            Missions = new ObservableCollection<MissionItem>();
            StickMissions = new ObservableCollection<MissionItem>();
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private Frame Frame
        {
            get
            {
                var fe = Window.Current.Content as FrameworkElement;
                return fe.GetChild<Frame>("MissionContentFrame");
            }
        }

        public ObservableCollection<WorkAddon> WorkAddons { get; }
        public ObservableCollection<MissionItem> StickMissions { get; }
        public ObservableCollection<MissionItem> Missions { get; }

        private WorkAddon selectedWork;
        public WorkAddon SelectedWork
        {
            get => selectedWork;
            set
            {
                if (selectedWork != value)
                {
                    selectedProject = null;
                    OnPropertyChanged(nameof(SelectedProject));
                    selectedStickProject = null;
                    OnPropertyChanged(nameof(SelectedStickProject));
                    selectedWork = value;
                    selectedProject = null;
                    OnPropertyChanged();
                    if (value.Features.Count > 0)
                    {
                        Frame.Navigate(typeof(MyMissionPage), value);
                    }
                }
            }
        }

        private MissionItem selectedProject;
        public MissionItem SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedWork = null;
                    OnPropertyChanged(nameof(SelectedWork));
                    selectedStickProject = null;
                    OnPropertyChanged(nameof(SelectedStickProject));
                    selectedProject = value;
                    OnPropertyChanged();
                }
            }
        }

        private MissionItem selectedStickProject;
        public MissionItem SelectedStickProject
        {
            get => selectedStickProject;
            set
            {
                if (selectedStickProject != value)
                {
                    selectedWork = null;
                    OnPropertyChanged(nameof(SelectedWork));
                    selectedProject = null;
                    OnPropertyChanged(nameof(SelectedProject));
                    selectedStickProject = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task GetWorkAddonsAsync()
        {
            string url = $"{Configuration.BaseAddress}/api/mission-vnext/work-addons";
            var resMsg = await HttpClient.GetAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                var data = await ReadHttpResponseMessageAsync<DataResult<WorkAddonDto>>(resMsg);
                if (data.Code == 200)
                {
                    foreach (var item in data.Data.Value)
                    {
                        item.Icon = WtfIconHelper.GetGlyph(item.Icon);
                        WorkAddons.Add(item);
                    }
                }
            }
            else
            {
                await HandleErrorStatusCodeAsync(resMsg);
            }
        }

        private async Task GetProjectNavAsync()
        {
            string url = $"{Configuration.BaseAddress}/api/mission-vnext/project-nav";
            var resMsg = await HttpClient.GetAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                var data = await ReadHttpResponseMessageAsync<DataResult<MissionNavDto>>(resMsg);
                if (data.Code == 200)
                {
                    foreach (var item in data.Data.MissionNav.Stick)
                    {
                        var project = data.Data.MissionItems.FirstOrDefault(p => p.Id == item);
                        if (project != null)
                        {
                            project.Color = ColorMap.GetNewColor(project.Color);
                            StickMissions.Add(project);
                        }
                    }
                    foreach (var item in data.Data.MissionNav.Items)
                    {
                        if (item.Items == null)
                        {
                            var project = data.Data.MissionItems.FirstOrDefault(p => p.Id == item.Id);
                            if (project != null)
                            {
                                project.Color = ColorMap.GetNewColor(project.Color);
                                Missions.Add(project);
                            }
                        }
                        else
                        {
                            foreach (var subItem in item.Items)
                            {
                                var project = data.Data.MissionItems.FirstOrDefault(p => p.Id == subItem.Id);
                                if (project != null)
                                {
                                    project.Color = ColorMap.GetNewColor(project.Color);
                                    Missions.Add(project);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                await HandleErrorStatusCodeAsync(resMsg);
            }
        }

        public async Task InitializeAsync()
        {
            await GetWorkAddonsAsync();
            await GetProjectNavAsync();
        }
    }
}
