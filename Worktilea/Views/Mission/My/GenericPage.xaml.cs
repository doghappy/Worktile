﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextWorkMyGeneric;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Common.Communication;

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
        private long _pageIndex;

        public IncrementalCollection<GridItem> GridItems { get; }

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

        private List<int> _pages;
        public List<int> Pages
        {
            get => _pages;
            set
            {
                if (_pages != value)
                {
                    _pages = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                }
            }
        }

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
            var data = await WtHttpClient.GetAsync<ApiMissionVnextWorkMyGeneric>(Uri);
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
                        BoldGlyph = WtIconHelper.GetBoldGlyph(state.Type),
                        Name = state.Name
                    },
                    Assignee = AvatarHelper.GetAvatar(item.Properties.Assignee.Value, AvatarSize.X40),
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
