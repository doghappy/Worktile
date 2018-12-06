using Newtonsoft.Json.Linq;
using System.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class PriorityReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            JObject jObj = JObject.FromObject(task);
            var value = TaskHelper.GetPropertyValue<string>(jObj, property.Key);
            if (value != null)
            {
                var priority = data.Data.References.Lookups.Priorities.Single(p => p.Id == value);
                kanban.Priority = WtColorHelper.GetNewBrush(priority.Color);
            }
        }
    }
}
