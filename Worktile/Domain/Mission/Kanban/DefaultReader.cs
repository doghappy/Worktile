using Newtonsoft.Json.Linq;
using System.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Infrastructure;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class DefaultReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            var kbp = new KanbanItemProperty
            {
                Name = property.Name
            };
            SetColor(kbp, setting.Color);

            JObject jObj = JObject.FromObject(task);
            string value = JTokenHelper.GetPropertyValue<string>(jObj, property.Key);
            if (value != null)
            {
                if (property.Lookup == null)
                {
                    kbp.Value = value;
                }
                else
                {
                    JObject lookup = JObject.FromObject(data.Data.References.Lookups);
                    var jItem = lookup[property.Lookup].Single(item => item["_id"].Value<string>() == value);
                    if (jItem is JObject obj)
                    {
                        if (obj.ContainsKey("name"))
                        {
                            kbp.Value = jItem["name"].Value<string>();
                            kanban.Properties.Add(kbp);
                        }
                        else if (obj.ContainsKey("text"))
                        {
                            kbp.Value = jItem["text"].Value<string>();
                            kanban.Properties.Add(kbp);
                        }
                    }
                }
            }
        }
    }
}
