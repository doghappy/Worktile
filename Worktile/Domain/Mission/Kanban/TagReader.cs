using Newtonsoft.Json.Linq;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Infrastructure;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using Worktile.ViewModels.Infrastructure;
using TaskState = Worktile.ApiModels.ApiMissionVnextKanbanContent.TaskState;

namespace Worktile.Domain.Mission.Kanban
{
    class TagReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            JObject jObj = JObject.FromObject(task);
            string[] tagIds = JTokenHelper.GetPropertyValue<string[]>(jObj, property.Key);
            if (tagIds.Length > 0)
            {
                foreach (var id in tagIds)
                {
                    var tag = data.Data.References.Lookups.Tags.Single(t => t.Id == id);
                    var kbp = new KanbanItemProperty
                    {
                        Value = tag.Name,
                        Foreground = new SolidColorBrush(Colors.White),
                        Background = WtColorHelper.GetNewBrush(tag.Color)
                    };
                    kanban.Properties.Add(kbp);
                }
            }
        }
    }
}
