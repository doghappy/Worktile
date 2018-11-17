using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextWorkMyGeneric;
using Worktile.Common;
using Worktile.Models;
using Worktile.Common;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.My
{
    public sealed partial class GenericPage : Page, INotifyPropertyChanged
    {
        public GenericPage()
        {
            InitializeComponent();
            GridItems = new IncrementalCollection<GridItem>(GetGridItemAsync);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IncrementalCollection<GridItem> GridItems { get; }

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

        private List<int> _pages;
        public List<int> Pages
        {
            get => _pages;
            set
            {
                _pages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
            }
        }

        private long _pageIndex;
        //public int PageIndex
        //{
        //    get => _pageIndex;
        //    set
        //    {
        //        _pageIndex = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageIndex)));
        //    }
        //}


        private string _uri;
        public string Uri
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
            Uri = "/api/mission-vnext/work/my/" + e.Parameter.ToString();
        }

        private async Task<IEnumerable<GridItem>> GetGridItemAsync()
        {
            IsActive = true;
            var list = new List<GridItem>();
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkMyGeneric>(Uri);
            int i = GridItems.Count;
            foreach (var item in data.Data.Value)
            {
                i++;
                var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == item.TaskStateId);
                var type = data.Data.References.TaskTypes.Single(t => t.Id == item.TaskTypeId);
                list.Add(new GridItem
                {
                    Id = item.Id,
                    RowId = i,
                    Title = item.Title,
                    Identifier = item.Identifier,
                    EndDate = item.Properties.Due.Value.Date?.ToShortDateString(),
                    State = new Models.TaskState
                    {
                        Foreground = WtColorHelper.GetNewColor(state.Color),
                        Glyph = WtIconHelper.GetGlyph(state.Type),
                        Name = state.Name
                    },
                    Assignee = CommonData.GetAvatar(item.Properties.Assignee.Value, 40),
                    TaskType = new Models.TaskType
                    {
                        Color = WtColorHelper.GetColorByClass(type.Icon),
                        Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
                        Name = type.Name
                    }
                });
            }
            GridItems.HasMoreItems = data.Data.PageCount - 1 > _pageIndex;
            _pageIndex++;
            IsActive = false;
            return list;
        }
    }

    public class GridItem
    {
        public string Id { get; set; }
        public int RowId { get; set; }
        public string Identifier { get; set; }
        public Models.TaskState State { get; set; }
        public string Title { get; set; }
        public Avatar Assignee { get; set; }
        public string EndDate { get; set; }
        public Models.TaskType TaskType { get; set; }
    }
}
