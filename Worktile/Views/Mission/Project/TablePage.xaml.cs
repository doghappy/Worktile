using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Domain.Mission.Table;
using Worktile.Enums;
using Worktile.Models.Mission.Table;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class TablePage : Page, INotifyPropertyChanged
    {
        public TablePage()
        {
            InitializeComponent();
            TableHeader = new ObservableCollection<HeaderCell>();
        }

        private string _addonId;
        private string _viewId;
        private string _taskIdentifierPrefix;

        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<HeaderCell> TableHeader { get; }

        List<List<Cell>> _rows;
        List<List<Cell>> Rows
        {
            get => _rows;
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rows)));
                }
            }
        }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            _addonId = parameters.AddonId;
            _viewId = parameters.ViewId;
            _taskIdentifierPrefix = parameters.TaskIdentifierPrefix;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestDataAsync();
            IsActive = false;
        }

        private async Task RequestDataAsync()
        {
            //string uri = $"/api/mission-vnext/table/{_addonId}/views/{_viewId}/content?pi=0&ps=20";
            string uri = $"/api/mission-vnext/table/{_addonId}/views/{_viewId}/content";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextTableContent>(uri);
            ReadHeaderItems(data);

            var reader = new PropertiesReader();
            Rows = reader.Read(_taskIdentifierPrefix, data, TableHeader);
        }

        /*
         * 表格视图性能需要优化，由于表格视图显示的列是动态的。
         * 渲染后，由于ListViewItem过于复杂，导致Item被点击时，
         * 动画不够流畅，同时也吃掉了很多内存。
         * 
         * 暂时没有想到好的解决方案，选择妥协。只显示固定的属性。
         */

        private void ReadHeaderItems(ApiMissionVnextTableContent data)
        {
            foreach (var item in data.Data.References.Columns)
            {
                var property = data.Data.References.Properties.Single(p => p.Id == item);
                var headerItem = new HeaderCell
                {
                    Text = property.Name,
                    Key = property.Key,
                    RowKey = property.RawKey,
                    Lookup = property.Lookup,
                    Type = property.Type
                };
                switch (property.Type)
                {
                    case WtTaskPropertyType.Number:
                        headerItem.Width = 100;
                        break;
                    case WtTaskPropertyType.Text:
                    case WtTaskPropertyType.DropDown:
                    case WtTaskPropertyType.Member:
                    case WtTaskPropertyType.Iteration:
                    case WtTaskPropertyType.Priority:
                    case WtTaskPropertyType.TaskType:
                    case WtTaskPropertyType.File:
                        headerItem.Width = 160;
                        break;
                    case WtTaskPropertyType.State:
                        headerItem.Width = 180;
                        break;
                    case WtTaskPropertyType.DateTime:
                    case WtTaskPropertyType.DateSpan:
                    case WtTaskPropertyType.MultiMember:
                    case WtTaskPropertyType.Workload:
                    case WtTaskPropertyType.Tag:
                    case WtTaskPropertyType.MultiSelect:
                        headerItem.Width = 200;
                        break;
                    case WtTaskPropertyType.MultiText:
                        headerItem.Width = 400;
                        break;
                }
                TableHeader.Add(headerItem);
            }
        }
    }
}
