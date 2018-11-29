using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextWorkAnalyticInsightProjectProgress;
using Worktile.Common;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class ProjectProgressPage : Page, INotifyPropertyChanged
    {
        public ProjectProgressPage()
        {
            InitializeComponent();
            ProjectItems = new IncrementalCollection<ProjectProgressItem>(ProjectItemsAsync);
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
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private int _projectCount;
        public int ProjectCount
        {
            get => _projectCount;
            set
            {
                if (_projectCount != value)
                {
                    _projectCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectCount)));
                }
            }
        }

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set
            {
                if (_totalCount != value)
                {
                    _totalCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCount)));
                }
            }
        }

        private int _pendingCount;
        public int PendingCount
        {
            get => _pendingCount;
            set
            {
                if (_pendingCount != value)
                {
                    _pendingCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PendingCount)));
                }
            }
        }

        private int _progressCount;
        public int ProgressCount
        {
            get => _progressCount;
            set
            {
                if (_progressCount != value)
                {
                    _progressCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressCount)));
                }
            }
        }

        private int _completedCount;
        public int CompletedCount
        {
            get => _completedCount;
            set
            {
                if (_completedCount != value)
                {
                    _completedCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompletedCount)));
                }
            }
        }

        private double _pointRate;
        public double PointRate
        {
            get => _pointRate;
            set
            {
                if (_pointRate != value)
                {
                    _pointRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PointRate)));
                }
            }
        }

        public IncrementalCollection<ProjectProgressItem> ProjectItems { get; }

        private async Task<IEnumerable<ProjectProgressItem>> ProjectItemsAsync()
        {
            string uri = $"/api/mission-vnext/work/analytic-insight/{_navId}/{_subNavId}/content?fpids=&fuids=&from=&to=&pi={_pageIndex}&ps={PageSize}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAnalyticInsightProjectProgress>(uri);
            if (!_totalPages.HasValue)
            {
                _totalPages = Convert.ToInt32(Math.Ceiling(data.Data.Value.Total * 1.0 / PageSize)) - 1;
            }
            if (!_isInitialized)
            {
                _isInitialized = true;
                ProjectCount = data.Data.Value.ItemCount;
                TotalCount = data.Data.Value.Total;
                PendingCount = data.Data.Value.Pending;
                ProgressCount = data.Data.Value.Progress;
                CompletedCount = data.Data.Value.Completed;
                PointRate = data.Data.Value.Point;
            }
            ProjectItems.HasMoreItems = _totalPages >= _pageIndex;
            _pageIndex++;
            IsActive = false;
            return data.Data.Value.Items.Select(i => new ProjectProgressItem
            {
                Id = i.ProjectId,
                Name = i.Name,
                Completed = i.Completed,
                Peding = i.Pending,
                Point = i.Point,
                Progress = i.Progress,
                Total = i.Total,
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

    public class ProjectProgressItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Peding { get; set; }
        public int Total { get; set; }
        public int Progress { get; set; }
        public int Completed { get; set; }
        public double Point { get; set; }
        public string Glyph { get; set; }
        public string Color { get; set; }
    }
}
