using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;

namespace Worktile.Domain.Mission.Kanban
{
    class TaskStateReader : PropertyReader
    {
        public override void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data)
        {
            kanban.State = new Models.TaskState
            {
                Name = state.Name,
                Foreground = WtColorHelper.GetNewColor(state.Color),
                BoldGlyph = WtIconHelper.GetBoldGlyph(state.Type)
            };
        }
    }
}
