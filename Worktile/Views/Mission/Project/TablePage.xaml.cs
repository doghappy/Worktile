using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Views.Mission.My;
using Worktile.Common.WtRequestClient;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class TablePage : Page, INotifyPropertyChanged
    {
        public TablePage()
        {
            InitializeComponent();
            GridItems = new IncrementalCollection<GridItem>(GetGridItemAsync);
        }

        private string _addonId;
        private string _viewId;
        private string _taskIdentifierPrefix;
        private int _pageIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        IncrementalCollection<GridItem> GridItems { get; }

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

        string _uri;
        string Uri
        {
            get
            {
                if (_pageIndex != 0)
                {
                    return _uri + "?pi=" + _pageIndex;
                }
                return _uri;
            }
            set => _uri = value;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            _addonId = parameters.AddonId;
            _viewId = parameters.ViewId;
            _taskIdentifierPrefix = parameters.TaskIdentifierPrefix;
            Uri = $"/api/mission-vnext/table/{_addonId}/views/{_viewId}/content";
        }

        /*
         * 表格视图性能需要优化，由于表格视图显示的列是动态的。
         * 渲染后，由于ListViewItem过于复杂，导致Item被点击时，
         * 动画不够流畅，同时也吃掉了很多内存。
         * 
         * 暂时没有想到好的解决方案，选择妥协。只显示固定的属性。
         */

        private async Task<IEnumerable<GridItem>> GetGridItemAsync()
        {
            IsActive = true;
            var list = new List<GridItem>();
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextTableContent>(Uri);
            int i = GridItems.Count;
            foreach (var item in data.Data.Value)
            {
                i++;
                var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == item.TaskStateId);
                var type = data.Data.References.TaskTypes.Single(t => t.Id == item.TaskTypeId);
               
                var gridItem = new GridItem
                {
                    Id = item.Id,
                    RowId = i,
                    Title = item.Title,
                    Identifier = item.Identifier,
                    State = new Models.TaskState
                    {
                        Foreground = WtColorHelper.GetNewColor(state.Color),
                        BoldGlyph = WtIconHelper.GetBoldGlyph(state.Type),
                        Name = state.Name
                    },
                    TaskType = new Models.TaskType
                    {
                        Color = WtColorHelper.GetColorByClass(type.Icon),
                        Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
                        Name = type.Name
                    }
                };

                if (item.Properties.ContainsKey("assignee"))
                {
                    string assineeUid = item.Properties["assignee"].Value<string>("value");
                    gridItem.Assignee = AvatarHelper.GetAvatar(assineeUid, AvatarSize.X40);
                }

                if (item.Properties.ContainsKey("due"))
                {
                    string timestamp = item.Properties["due"]["value"].Value<string>("date");
                    if (timestamp != null)
                    {
                        gridItem.EndDate = WtDateTimeHelper.ToWtKanbanDate(timestamp);
                    }
                }
                list.Add(gridItem);
            }
            GridItems.HasMoreItems = (data.Data.PageCount ?? 0) - 1 > _pageIndex;
            _pageIndex++;
            IsActive = false;
            return list;
        }
    }
}
