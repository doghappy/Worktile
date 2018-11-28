using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using Worktile.WtRequestClient;

namespace Worktile.ViewModels.Mission.Project
{
    public class KanbanViewModel : BindableBase
    {
        public KanbanViewModel()
        {
            KanBanGroups = new ObservableCollection<KanbanGroup>();
            _groupedIds = new List<string>();
            _notInStackProperties = new[] { "tag", "task_state_id", "title", "task_type_id", "updated_at", "created_at", "attachment", "assignee", "priority" };
        }

        readonly string[] _notInStackProperties;

        private List<string> _groupedIds;
        public string AddonId { get; set; }
        public string ViewId { get; set; }
        public string TaskIdentifierPrefix { get; set; }

        public ObservableCollection<KanbanGroup> KanBanGroups { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public async Task RequestContentAsync()
        {
            string uri = $"/api/mission-vnext/kanban/{AddonId}/views/{ViewId}/content";
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

            KanbanPageHelper.ReadForProgressBar(kbGroup, state.Type);

            dynamic props = task.Properties.ToObject<object>();

            var item = new KanbanItem
            {
                Id = task.Id,
                Title = task.Title,
                AttachmentCount = GetAttachmentCount(task),
                Priority = GetPriorityBrush(props.priority, data),
                Properties = GetProperties(type.ShowSettings, data, task),
                State = new Models.TaskState
                {
                    Name = state.Name,
                    Foreground = WtColorHelper.GetNewColor(state.Color),
                    Glyph = WtIconHelper.GetGlyph(state.Type)
                },
                TaskType = new Models.TaskType
                {
                    Name = type.Name,
                    Color = WtColorHelper.GetColorByClass(type.Icon),
                    Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
                }
            };

            if (props.assignee != null && props.assignee.value != null)
            {
                item.Avatar = CommonData.GetAvatar((string)props.assignee.value, 40);
            }
            kbGroup.Items.Add(item);
        }

        private List<KanbanItemProperty> GetProperties(List<ShowSetting> showSettings, ApiMissionVnextKanbanContent data, ValueElement task)
        {
            var list = new List<KanbanItemProperty>();
            foreach (var item in showSettings)
            {
                var property = data.Data.References.Properties.Single(p => p.Id == item.TaskPropertyId);
                if (property.RawKey == "tag")
                {
                    var arr = task.Properties["tag"]["value"] as JArray;
                    foreach (var tagId in arr)
                    {
                        var tag = data.Data.References.Lookups.Tags.Single(t => t.Id == tagId.Value<string>());
                        var kbp = new KanbanItemProperty
                        {
                            Value = tag.Name,
                            Foreground = Application.Current.Resources["SystemControlForegroundBaseHighBrush"] as SolidColorBrush,
                            Background = WtColorHelper.GetNewBrush(tag.Color)
                        };
                        list.Add(kbp);
                    }
                }
                else if (!_notInStackProperties.Contains(property.RawKey))
                {
                    string rawValue = GetPropertyValue(task, property);
                    var kbp = new KanbanItemProperty
                    {
                        Name = property.Name,
                        Value = ParsePropertyValue(property, data, item, rawValue)
                    };
                    if (kbp.Value != null)
                    {
                        if (string.IsNullOrEmpty(item.Color))
                        {
                            kbp.Foreground = Application.Current.Resources["SystemControlForegroundBaseMediumBrush"] as SolidColorBrush;
                            kbp.Background = Application.Current.Resources["SystemControlForegroundBaseLowBrush"] as SolidColorBrush;
                        }
                        else
                        {
                            string color = WtColorHelper.GetNewColor(item.Color);
                            kbp.Foreground = WtColorHelper.GetSolidColorBrush(color);
                            kbp.Background = WtColorHelper.GetSolidColorBrush(color.Insert(1, "1A"));
                        }
                        list.Add(kbp);
                    }
                }
            }
            return list;
        }

        private SolidColorBrush GetPriorityBrush(dynamic priority, ApiMissionVnextKanbanContent data)
        {
            if (priority == null)
            {
                return null;
            }
            else
            {
                string value = priority.value;
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                else
                {
                    var item = data.Data.References.Lookups.Priorities.Single(p => p.Id == value);
                    return WtColorHelper.GetSolidColorBrush(WtColorHelper.GetNewColor(item.Color));
                }
            }
        }

        private int GetAttachmentCount(ValueElement task)
        {
            if (task.Properties.ContainsKey("attachment"))
            {
                var attachmentValues = task.Properties["attachment"]["value"] as JArray;
                return attachmentValues.Count;
            }
            return 0;
        }

        private string GetPropertyValue(ValueElement task, WtTaskProperty property)
        {
            string[] keys = property.Key.Split('.');

            JToken obj = JObject.FromObject(task);
            foreach (var k in keys)
            {
                obj = obj[k];
            }
            if (obj == null)
            {
                return null;
            }
            else
            {
                if (obj.Type == JTokenType.Object)
                {
                    var sub = obj as JObject;
                    if (sub.ContainsKey("date"))
                    {
                        obj = obj["date"];
                    }
                    else
                    {
                        return null;
                    }
                }
                return obj.Value<string>();
            }
        }

        private string ParsePropertyValue(WtTaskProperty property, ApiMissionVnextKanbanContent data, ShowSetting setting, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (property.Type == WtTaskPropertyType.DateTime)
                {
                    DateTime dateTime = WtDateTimeHelper.GetDateTime(value);
                    value = WtDateTimeHelper.GetDateTime(value).ToWtKanbanDate();
                    if (property.RawKey == "due" && dateTime <= DateTime.Now)
                        setting.Color = "#ff5b57";
                    else
                        setting.Color = null;
                }
                else
                {
                    switch (property.RawKey)
                    {
                        case "created_by":
                            value = CommonData.GetAvatar(value, 40)?.DisplayName;
                            break;
                        case "identifier":
                            value = TaskIdentifierPrefix + value;
                            break;
                    }
                }
                if (property.Lookup != null
                    && !string.IsNullOrEmpty(value)
                    && property.From == WtTaskPropertyFrom.Custom)
                {
                    JToken obj = JObject.FromObject(data.Data.References.Lookups);
                    JArray arr = obj[property.Lookup] as JArray;
                    return arr.First(a => a["_id"].Value<string>() == value)["text"].Value<string>();
                }
            }
            return value;
        }
    }
}
