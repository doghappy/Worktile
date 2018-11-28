using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Worktile.ApiModels.ApiMissionVnextWorkAnalyticInsightGroups;
using Worktile.Models.Mission.AnalyticInsight;
using Worktile.WtRequestClient;

namespace Worktile.ViewModels.Mission.AnalyticInsight
{
    public class IndexViewModel : ViewModel
    {
        public IndexViewModel()
        {
            TopNavItems = new ObservableCollection<TopNavItem>();
        }

        public ObservableCollection<TopNavItem> TopNavItems { get; }

        private string _topNavIcon;
        public string TopNavIcon
        {
            get => _topNavIcon;
            set => SetProperty(ref _topNavIcon, value);
        }

        private string _topNavName;
        public string TopNavName
        {
            get => _topNavName;
            set => SetProperty(ref _topNavName, value);
        }

        public async Task RequestApiDataAsync()
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
        }
    }
}
