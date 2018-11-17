using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextWorkAnalyticInsightProjectDelay;
using Worktile.Common;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class ProjectDelayPage : Page, INotifyPropertyChanged
    {
        public ProjectDelayPage()
        {
            InitializeComponent();
            ProjectItems = new IncrementalCollection<ProjectDelayItem>(ProjectItemsAsync);
            IsActive = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _navId;
        private string _subNavId;
        const int PageSize = 20;
        private int? _totalPages;
        private int _pageIndex;
        private bool _isInitialized;

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

        private int _projectCount;
        public int ProjectCount
        {
            get => _projectCount;
            set
            {
                _projectCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectCount)));
            }
        }

        private int _followCount;
        public int FollowCount
        {
            get => _followCount;
            set
            {
                _followCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FollowCount)));
            }
        }

        private int _pendingCount;
        public int PendingCount
        {
            get => _pendingCount;
            set
            {
                _pendingCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PendingCount)));
            }
        }

        private int _progressCount;
        public int ProgressCount
        {
            get => _progressCount;
            set
            {
                _progressCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressCount)));
            }
        }

        private int _delayCount;
        public int DelayCount
        {
            get => _delayCount;
            set
            {
                _delayCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DelayCount)));
            }
        }

        private double _pointRate;
        public double PointRate
        {
            get => _pointRate;
            set
            {
                _pointRate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PointRate)));
            }
        }

        public IncrementalCollection<ProjectDelayItem> ProjectItems { get; }

        private async Task<IEnumerable<ProjectDelayItem>> ProjectItemsAsync()
        {
            string uri = $"/api/mission-vnext/work/analytic-insight/{_navId}/{_subNavId}/content?fpids=&fuids=&from=&to=&pi={_pageIndex}&ps={PageSize}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAnalyticInsightProjectDelay>(uri);
            if (!_totalPages.HasValue)
            {
                _totalPages = Convert.ToInt32(Math.Ceiling(data.Data.Value.ItemCount * 1.0 / PageSize)) - 1;
            }
            if (!_isInitialized)
            {
                _isInitialized = true;
                ProjectCount = data.Data.Value.ItemCount;
                FollowCount = data.Data.Value.Follow;
                PendingCount = data.Data.Value.Pending;
                ProgressCount = data.Data.Value.Progress;
                DelayCount = data.Data.Value.DelayCount;
                PointRate = data.Data.Value.Point;
            }
            ProjectItems.HasMoreItems = _totalPages >= _pageIndex;
            _pageIndex++;
            IsActive = false;
            return data.Data.Value.Items.Select(i => new ProjectDelayItem
            {
                Id = i.ProjectId,
                Name = i.Name,
                Delay = i.DelayCount,
                Peding = i.Pending,
                Point = i.Point,
                Progress = i.Progress,
                Follow = i.Follow,
                Glyph = i.Visibility == 1 ? "\ue70c" : "\ue667",
                Color = WtColorHelper.GetNewColor(i.Color)
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic param = e.Parameter;
            _navId = param.NavId;
            _subNavId = param.SubNavId;
        }
    }

    public class ProjectDelayItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Peding { get; set; }
        public int Follow { get; set; }
        public int Progress { get; set; }
        public int Delay { get; set; }
        public double Point { get; set; }
        public string Glyph { get; set; }
        public string Color { get; set; }
    }
}
