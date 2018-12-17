using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission;

namespace Worktile.Views.Mission.Project.Detail
{
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
            Properties = new ObservableCollection<PropertyItem>();
            _ignorePropertyKeys = new[] {
                "identifier",
                "title",
                "task_state_id",
                "task_type_id",
                "created_by",
                "created_at",
                "updated_at",
                "updated_at",
                "assignee",
                "start",
                "due",
                "attachment",
                "child",
                "workload",
                "relation"
            };
        }

        private Data _task;
        readonly string[] _ignorePropertyKeys;

        ObservableCollection<PropertyItem> Properties { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _task = e.Parameter as Data;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Add(new PropertyItem
            {
                Header = "项目名称",
                Value = _task.References.Project.Name,
                Glyph = WtIconHelper.GetGlyph(_task.References.Project.Visibility),
                IsReadonly = true,
                Color = new SolidColorBrush((Color)Resources["SystemAccentColor"])
            });

            string taskTypeId = _task.Value.TaskTypeId;
            var taskType = _task.References.TaskTypes.Single(t => t.Id == taskTypeId);
            Properties.Add(new PropertyItem
            {
                Header = "任务类型",
                Value = taskType.Name,
                Glyph = WtIconHelper.GetGlyph("wtf-type-" + taskType.Icon),
                IsReadonly = true,
                Color = WtColorHelper.GetSolidColorBrush(WtColorHelper.GetColorByClass(taskType.Icon))
            });

            foreach (var prop in _task.References.Properties)
            {
                if (!_ignorePropertyKeys.Contains(prop.RawKey))
                {
                    var item = new PropertyItem
                    {
                        Header = prop.Name,
                        //Type = prop.Type
                    };
                    JObject jObj = JObject.FromObject(_task.Value);
                    switch (prop.Type)
                    {
                        case WtTaskPropertyType.Number:
                            {
                                item.Value = TaskHelper.GetPropertyValue<string>(jObj, prop.Key);
                                item.Control = nameof(TextBox);
                            }
                            break;
                    }
                    Properties.Add(item);
                }
            }
        }
    }

    class PropertyItem
    {
        public string Header { get; set; }
        public string Value { get; set; }
        public string Glyph { get; set; }
        public SolidColorBrush Color { get; set; }
        public bool IsReadonly { get; set; }
        //public WtTaskPropertyType Type { get; set; }
        public List<DropdownItem> DataSource { get; set; }
        public string Control { get; set; }
    }

    class DropdownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
