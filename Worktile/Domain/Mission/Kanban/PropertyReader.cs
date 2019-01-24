using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Models.Kanban;
using Worktile.Models.Mission.WtTask;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.Domain.Mission.Kanban
{
    abstract class PropertyReader
    {
        public abstract void Read(KanbanItem kanban, WtTaskProperty property, ValueElement task, TaskState state, ShowSetting setting, ApiMissionVnextKanbanContent data);

        protected void SetColor(KanbanItemProperty kbp, string color)
        {
            if (color == null)
            {
                kbp.Foreground = WtColorHelper.GetSolidColorBrush("#aaaaaa");
                kbp.Background = WtColorHelper.GetSolidColorBrush("#33aaaaaa");
            }
            else
            {
                string newColor = WtColorHelper.GetNewColor(color);
                kbp.Foreground = WtColorHelper.GetSolidColorBrush(newColor);
                kbp.Background = WtColorHelper.GetSolidColorBrush(newColor.Insert(1, "33"));
            }
        }
    }
}
