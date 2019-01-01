using Newtonsoft.Json.Linq;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class MemberReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            var kbp = new KanbanItemProperty
            {
                Name = property.Name
            };
            SetColor(kbp, setting.Color);

            JObject jObj = JObject.FromObject(task);
            string uid = TaskHelper.GetPropertyValue<string>(jObj, property.Key);
            if (!string.IsNullOrEmpty(uid))
            {
                var avatar = AvatarHelper.GetAvatar(uid, 40);
                if (property.RawKey == "assignee")
                {
                    kanban.Due = avatar;
                }
                else
                {
                    kbp.Value = avatar.DisplayName;
                    kanban.Properties.Add(kbp);
                }
            }
        }
    }
}
