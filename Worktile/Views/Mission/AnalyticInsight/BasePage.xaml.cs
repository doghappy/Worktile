using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public abstract partial class BasePage : Page, INotifyPropertyChanged
    {
        public BasePage()
        {
            InitializeComponent();
            DataItems = new IncrementalCollection<AnalyticInsightItem>(LoadItemsAsync);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public const int PageSize = 20;
        private int? _totalPages;
        private int _pageIndex;
        private bool _isInitialized;

        public IncrementalCollection<AnalyticInsightItem> DataItems { get; }

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

        protected string NavId { get; set; }
        protected string SubNavId { get; set; }

        protected abstract int GetTotalByData(object data);

        protected abstract void ReadHeaderInfo(object data);

        protected abstract IEnumerable<AnalyticInsightItem> GetDataItems(object data);

        protected async Task<IEnumerable<AnalyticInsightItem>> LoadItemsAsync()
        {
            string uri = $"/api/mission-vnext/work/analytic-insight/{NavId}/{SubNavId}/content?fpids=&fuids=&from=&to=&pi={_pageIndex}&ps={PageSize}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<object>(uri);
            if (!_totalPages.HasValue)
            {
                _totalPages = Convert.ToInt32(Math.Ceiling(GetTotalByData(data) * 1.0 / PageSize)) - 1;
            }
            if (!_isInitialized)
            {
                _isInitialized = true;
                ReadHeaderInfo(data);
            }
            DataItems.HasMoreItems = _totalPages >= _pageIndex;
            _pageIndex++;
            IsActive = false;
            return GetDataItems(data);
        }
    }

    public class AnalyticInsightItem
    {
        public string Id { get; set; }
        public int Peding { get; set; }
        public int Total { get; set; }
        public int Progress { get; set; }
        public int Completed { get; set; }
        public double Point { get; set; }
    }
}
