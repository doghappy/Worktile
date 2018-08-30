using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Models.Project;
using Worktile.WindowsUI.Models.Results;

namespace Worktile.WindowsUI.ViewModels.Project
{
    public class ProjectViewModel : ViewModel
    {
        public ProjectViewModel()
        {
            WorkAddons = new ObservableCollection<WorkAddon>();
            Projects = new ObservableCollection<Models.Project.Project>();
            StickProjects = new ObservableCollection<Models.Project.Project>();
        }

        public ObservableCollection<WorkAddon> WorkAddons { get; }
        public ObservableCollection<Models.Project.Project> StickProjects { get; }
        public ObservableCollection<Models.Project.Project> Projects { get; }

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
                var data = await ReadHttpResponseMessageAsync<DataResult<ProjectNavDto>>(resMsg);
                if (data.Code == 200)
                {
                    foreach (var item in data.Data.ProjectNav.Stick)
                    {
                        var project = data.Data.Projects.FirstOrDefault(p => p.Id == item);
                        if (project != null)
                        {
                            StickProjects.Add(project);
                        }
                    }
                    foreach (var item in data.Data.ProjectNav.Items)
                    {
                        if (item.Items == null)
                        {
                            var project = data.Data.Projects.FirstOrDefault(p => p.Id == item.Id);
                            if (project != null)
                            {
                                Projects.Add(project);
                            }
                        }
                        else
                        {
                            foreach (var subItem in item.Items)
                            {
                                var project = data.Data.Projects.FirstOrDefault(p => p.Id == subItem.Id);
                                if (project != null)
                                {
                                    Projects.Add(project);
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
