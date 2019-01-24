using Newtonsoft.Json.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Infrastructure;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;

namespace Worktile.Domain.Mission.Kanban
{
    class AttachmentReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            JObject jObj = JObject.FromObject(task);
            string[] files = JTokenHelper.GetPropertyValue<string[]>(jObj, property.Key);
            kanban.AttachmentCount = files.Length;
        }
    }
}
