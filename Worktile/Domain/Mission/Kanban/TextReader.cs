using Newtonsoft.Json.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Infrastructure;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;

namespace Worktile.Domain.Mission.Kanban
{
    class TextReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            if (property.RawKey == "title")
            {
                kanban.Title = task.Title;
            }
            else
            {
                JObject jObj = JObject.FromObject(task);
                string value = JTokenHelper.GetPropertyValue<string>(jObj, property.Key);
                if (value != null)
                {
                    var kbp = new KanbanItemProperty
                    {
                        Name = property.Name,
                        Value = value
                    };
                    SetColor(kbp, setting.Color);
                    kanban.Properties.Add(kbp);
                }
            }
        }
    }
}
