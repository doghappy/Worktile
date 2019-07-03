using System.Collections.Generic;
using System.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Kanban;

namespace Worktile.Domain.Mission.Kanban
{
    class KanbanDataReader
    {
        private List<string> _groupedIds;

        public List<KanbanGroup> GetData(ApiMissionVnextKanbanContent data)
        {
            var list = new List<KanbanGroup>();
            _groupedIds = new List<string>();

            foreach (var item in data.Data.References.Groups)
            {
                _groupedIds.AddRange(item.TaskIds);
                var kbGroup = new KanbanGroup
                {
                    Header = item.Name
                };
                foreach (var taskId in item.TaskIds)
                {
                    ReadKanbanItem(data, kbGroup, taskId);
                }
                list.Add(kbGroup);
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
            list.Insert(0, unGroup);
            return list;
        }

        private void ReadKanbanItem(ApiMissionVnextKanbanContent data, KanbanGroup kbGroup, string taskId)
        {
            var task = data.Data.Value.Single(v => v.Id == taskId);
            var type = data.Data.References.TaskTypes.Single(t => t.Id == task.TaskTypeId);
            var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == task.TaskStateId);

            KanbanPageHelper.ReadForProgressBar(kbGroup, state.Type);

            var kanban = new KanbanItem
            {
                Id = task.Id,
                TaskType = new Models.TaskType
                {
                    Name = type.Name,
                    Color = WtColorHelper.GetColorByClass(type.Icon),
                    Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
                },
                Properties = new List<KanbanItemProperty>()
            };

            foreach (var item in type.ShowSettings)
            {
                var property = data.Data.References.Properties.Single(p => p.Id == item.TaskPropertyId);
                PropertyReader reader = null;
                switch (property.Type)
                {
                    case WtTaskPropertyType.Text:
                        reader = new TextReader();
                        break;
                    case WtTaskPropertyType.DateTime:
                        reader = new DateTimeReader();
                        break;
                    case WtTaskPropertyType.DateSpan:
                        reader = new DateSpanReader();
                        break;
                    case WtTaskPropertyType.Member:
                        reader = new MemberReader();
                        break;
                    case WtTaskPropertyType.Workload:
                        reader = new WorkloadReader();
                        break;
                    case WtTaskPropertyType.State:
                        reader = new TaskStateReader();
                        break;
                    case WtTaskPropertyType.Tag:
                        reader = new TagReader();
                        break;
                    case WtTaskPropertyType.Priority:
                        reader = new PriorityReader();
                        break;
                    case WtTaskPropertyType.File:
                        reader = new AttachmentReader();
                        break;
                    case WtTaskPropertyType.TaskType:
                        continue;
                    default:
                        reader = new DefaultReader();
                        break;
                }
                reader.Read(kanban, property, task, state, item, data);
            }

            kbGroup.Items.Add(kanban);
        }
    }
}
