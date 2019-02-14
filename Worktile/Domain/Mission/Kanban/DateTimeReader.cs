using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Infrastructure;
using Worktile.Models;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class DateTimeReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            var kbp = new KanbanItemProperty
            {
                Name = property.Name
            };

            SetColor(kbp, setting.Color);

            JObject jObj = JObject.FromObject(task);
            WtDate date = null;
            if (property.Key.Count(k => k == '.') > 0)
            {
                date = JTokenHelper.GetPropertyValue<WtDate>(jObj, property.Key);
            }
            else
            {
                date = new WtDate();
                string timestamp = jObj[property.Key].Value<string>();
                if (timestamp != null)
                {
                    date.Date = WtDateTimeHelper.GetDateTime(timestamp);
                }
            }

            if (date.Date.HasValue)
            {
                kbp.Value = date.Date.Value.ToWtKanbanDate();
                if (property.RawKey == "due" && date.Date.Value < DateTime.Now)
                {
                    kbp.Foreground = WtColorHelper.DangerColor;
                    kbp.Background = WtColorHelper.DangerColor1A;
                }
                kanban.Properties.Add(kbp);
            }
        }
    }
}
