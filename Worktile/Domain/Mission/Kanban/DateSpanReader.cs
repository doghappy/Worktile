using Newtonsoft.Json.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class DateSpanReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            var kbp = new KanbanItemProperty
            {
                Name = property.Name
            };

            SetColor(kbp, setting.Color);

            JObject jObj = JObject.FromObject(task);
            var span = TaskHelper.GetPropertyValue<DateSpan>(jObj, property.Key);

            if (span.Begin.Date.HasValue && span.End.Date.HasValue)
            {
                kbp.Value = WtDateTimeHelper.ToWtKanbanDate(span.Begin.Date.Value)
                    + " ~ "
                    + WtDateTimeHelper.ToWtKanbanDate(span.End.Date.Value);
                kanban.Properties.Add(kbp);
            }
        }
    }
}
