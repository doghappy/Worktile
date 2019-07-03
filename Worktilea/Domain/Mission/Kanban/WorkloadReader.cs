using Newtonsoft.Json.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models.Kanban;
using Worktile.Models.Mission;
using Worktile.Models.Mission.WtTask;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class WorkloadReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            var kbp = new KanbanItemProperty
            {
                Name = "实际工时"
            };
            SetColor(kbp, setting.Color);

            JObject jObj = JObject.FromObject(task);
            var workload = JTokenHelper.GetPropertyValue<WorkloadValue>(jObj, property.Key);
            if (workload.Actual != 0)
            {
                kbp.Value = workload.Actual.ToString();
                kanban.Properties.Add(kbp);
            }
        }
    }
}
