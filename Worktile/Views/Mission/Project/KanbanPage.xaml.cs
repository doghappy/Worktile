using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models;
using Worktile.Models.Kanban;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class KanbanPage : Page, INotifyPropertyChanged
    {
        public KanbanPage()
        {
            InitializeComponent();
            KanBanGroups = new ObservableCollection<KanbanGroup>();
            _groupedIds = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _addonId;
        private string _viewId;
        private bool _isPageLoaed;
        private List<string> _groupedIds;

        public ObservableCollection<KanbanGroup> KanBanGroups { get; }

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            _addonId = parameters.AddonId;
            _viewId = parameters.ViewId;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestContentAsync();
            IsActive = false;
            _isPageLoaed = true;
        }

        private async Task RequestContentAsync()
        {
            string uri = $"/api/mission-vnext/kanban/{_addonId}/views/{_viewId}/content";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextKanbanContent>(uri);

            foreach (var item in data.Data.References.Groups)
            {
                var kbGroup = new KanbanGroup
                {
                    Header = item.Name
                };
                _groupedIds.AddRange(item.TaskIds);
                foreach (var taskId in item.TaskIds)
                {
                    ReadKanbanItem(data, kbGroup, taskId);
                }
                KanBanGroups.Add(kbGroup);
            }

            var unGroup = new KanbanGroup
            {
                Header = "未分组"
            };
            var unGroupIds = data.Data.Value.Where(v => !_groupedIds.Contains(v.Id));
            foreach (var item in unGroupIds)
            {
                ReadKanbanItem(data, unGroup, item.Id);
            }
            KanBanGroups.Insert(0, unGroup);
        }

        private void ReadKanbanItem(ApiMissionVnextKanbanContent data, KanbanGroup kbGroup, string taskId)
        {
            var task = data.Data.Value.Single(v => v.Id == taskId);
            var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == task.TaskStateId);
            var type = data.Data.References.TaskTypes.Single(t => t.Id == task.TaskTypeId);
            switch (state.Type)
            {
                case 1: kbGroup.NotStarted++; break;
                case 2: kbGroup.Processing++; break;
                case 3: kbGroup.Completed++; break;
            }
            //kbGroup.Items.Add(new KanBanItem
            //{
            //    Id = task.Id,
            //    Title = task.Title,
            //    Identifier = task.Identifier,
            //    Avatar = CommonData.GetAvatar(task.Properties.Assignee.Value, 40),
            //    Priority = GetPriorityBrush(task.Properties.Priority, data),
            //    ShowSettings = GetShowSettings(type.ShowSettings, data),
            //    State = new Models.TaskState
            //    {
            //        Name = state.Name,
            //        Foreground = WtColorHelper.GetNewColor(state.Color),
            //        Glyph = WtIconHelper.GetGlyph(state.Type)
            //    },
            //    TaskType = new Models.TaskType
            //    {
            //        Name = type.Name,
            //        Color = WtColorHelper.GetColorByClass(type.Icon),
            //        Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
            //    }
            //});
        }

        //private List<Models.ShowSetting> GetShowSettings(List<ApiModel.ApiMissionVnextKanbanContent.ShowSetting> showSettings, ApiMissionVnextKanbanContent data)
        //{
        //    var list = new List<Models.ShowSetting>();
        //    foreach (var item in showSettings)
        //    {
        //        var prop = data.Data.References.Properties.Single(p => p.Id == item.TaskPropertyId);
        //        list.Add(new Models.ShowSetting
        //        {
        //            Id = item.TaskPropertyId,
        //            Foreground = item.Color,
        //            From = prop.From,
        //            Key = prop.Key,
        //            Name = prop.Name,
        //            PropertyKey = prop.PropertyKey,
        //            RawKey = prop.RawKey,
        //            Type = prop.Type
        //        });
        //    }
        //    return list;
        //}

        private SolidColorBrush GetPriorityBrush(PropertiesPriority priority, ApiMissionVnextKanbanContent data)
        {
            if (string.IsNullOrEmpty(priority.Value))
            {
                return null;
            }
            else
            {
                var item = data.Data.References.Lookups.Priorities.Single(p => p.Id == priority.Value);
                return WtColorHelper.GetSolidColorBrush(WtColorHelper.GetNewColor(item.Color));
            }
        }

        private async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sp = MyGrid.GetChild<StackPanel>("MissionHeader");
            var lvs = MyGrid.GetChildren<ListView>("DataList");
            if (sp != null && lvs.Any())
            {
                foreach (var item in lvs)
                {
                    item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    if (_isPageLoaed)
                    {
                        sp = MyGrid.GetChild<StackPanel>("MissionHeader");
                        lvs = MyGrid.GetChildren<ListView>("DataList");
                        if (sp != null && lvs.Any())
                        {
                            foreach (var item in lvs)
                            {
                                item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                            }
                            return;
                        }
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}
