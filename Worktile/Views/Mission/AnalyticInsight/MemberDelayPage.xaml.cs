using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextWorkAnalyticInsightMemberDelay;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Common.WtRequestClient;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class MemberDelayPage : Page, INotifyPropertyChanged
    {
        public MemberDelayPage()
        {
            InitializeComponent();
            MemberItems = new IncrementalCollection<MemberDelayItem>(MemberItemsAsync);
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

        private int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            set
            {
                if (_itemCount != value)
                {
                    _itemCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ItemCount)));
                }
            }
        }

        private int _followCount;
        public int FollowCount
        {
            get => _followCount;
            set
            {
                if (_followCount != value)
                {
                    _followCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FollowCount)));
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

        private int _delayCount;
        public int DelayCount
        {
            get => _delayCount;
            set
            {
                if (_delayCount != value)
                {
                    _delayCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DelayCount)));
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

        public IncrementalCollection<MemberDelayItem> MemberItems { get; }

        private async Task<IEnumerable<MemberDelayItem>> MemberItemsAsync()
        {
            string uri = $"/api/mission-vnext/work/analytic-insight/{_navId}/{_subNavId}/content?fpids=&fuids=&from=&to=&pi={_pageIndex}&ps={PageSize}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAnalyticInsightMemberDelay>(uri);
            if (!_totalPages.HasValue)
            {
                _totalPages = Convert.ToInt32(Math.Ceiling(data.Data.Value.ItemCount * 1.0 / PageSize)) - 1;
            }
            if (!_isInitialized)
            {
                _isInitialized = true;
                ItemCount = data.Data.Value.ItemCount;
                FollowCount = data.Data.Value.Follow;
                PendingCount = data.Data.Value.Pending;
                ProgressCount = data.Data.Value.Progress;
                DelayCount = data.Data.Value.DelayCount;
                PointRate = data.Data.Value.Point;
            }
            MemberItems.HasMoreItems = _totalPages >= _pageIndex;
            _pageIndex++;
            IsActive = false;
            return data.Data.Value.Items.Select(i => new MemberDelayItem
            {
                Delay = i.DelayCount,
                Pending = i.Pending,
                Point = i.Point,
                Progress = i.Progress,
                Follow = i.Follow,
                Avatar = AvatarHelper.GetAvatar(i.Uid, AvatarSize.X40)
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

    public class MemberDelayItem
    {
        public int Pending { get; set; }
        public int Follow { get; set; }
        public int Progress { get; set; }
        public int Delay { get; set; }
        public double Point { get; set; }
        public Avatar Avatar { get; set; }
    }
}
