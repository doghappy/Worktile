using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextWorkAnalyticInsightMemberProgress;
using Worktile.Common;
using Worktile.Models;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class MemberProgressPage : Page, INotifyPropertyChanged
    {
        public MemberProgressPage()
        {
            InitializeComponent();
            MemberItems = new IncrementalCollection<MemberProgressItem>(MemberItemsAsync);
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

        private int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            set
            {
                _itemCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemCount)));
            }
        }

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set
            {
                _totalCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCount)));
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

        private int _completedCount;
        public int CompletedCount
        {
            get => _completedCount;
            set
            {
                _completedCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompletedCount)));
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

        public IncrementalCollection<MemberProgressItem> MemberItems { get; }

        private async Task<IEnumerable<MemberProgressItem>> MemberItemsAsync()
        {
            string uri = $"/api/mission-vnext/work/analytic-insight/{_navId}/{_subNavId}/content?fpids=&fuids=&from=&to=&pi={_pageIndex}&ps={PageSize}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAnalyticInsightMemberProgress>(uri);
            if (!_totalPages.HasValue)
            {
                _totalPages = Convert.ToInt32(Math.Ceiling(data.Data.Value.Total * 1.0 / PageSize)) - 1;
            }
            if (!_isInitialized)
            {
                _isInitialized = true;
                ItemCount = data.Data.Value.ItemCount;
                TotalCount = data.Data.Value.Total;
                PendingCount = data.Data.Value.Pending;
                ProgressCount = data.Data.Value.Progress;
                CompletedCount = data.Data.Value.Completed;
                PointRate = data.Data.Value.Point;
            }
            MemberItems.HasMoreItems = _totalPages >= _pageIndex;
            _pageIndex++;
            IsActive = false;
            return data.Data.Value.Items.Select(i => new MemberProgressItem
            {
                Completed = i.Completed,
                Peding = i.Pending,
                Point = i.Point,
                Progress = i.Progress,
                Total = i.Total,
                Avatar = CommonData.GetAvatar(i.Uid, 40)
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

    public class MemberProgressItem
    {
        public int Peding { get; set; }
        public int Total { get; set; }
        public int Progress { get; set; }
        public int Completed { get; set; }
        public double Point { get; set; }
        public Avatar Avatar { get; set; }
    }
}
