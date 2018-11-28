using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Kanban;

namespace Worktile.Common
{
    public static class KanbanPageHelper
    {
        public static async Task KanbanGridAdaptiveAsync(Grid grid)
        {
            var sp = grid.GetChild<StackPanel>("MissionHeader");
            var lvs = grid.GetChildren<ListView>("DataList");
            if (sp != null && lvs.Any())
            {
                foreach (var item in lvs)
                {
                    item.MaxHeight = grid.ActualHeight - 24 - sp.ActualHeight;
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    sp = grid.GetChild<StackPanel>("MissionHeader");
                    lvs = grid.GetChildren<ListView>("DataList");
                    if (sp != null && lvs.Any())
                    {
                        foreach (var item in lvs)
                        {
                            item.MaxHeight = grid.ActualHeight - 24 - sp.ActualHeight;
                        }
                        return;
                    }
                    await Task.Delay(100);
                }
            }
        }

        public static void ReadForProgressBar(KanbanGroup kbGroup, int state)
        {
            switch (state)
            {
                case 1: kbGroup.NotStarted++; break;
                case 2: kbGroup.Processing++; break;
                case 3: kbGroup.Completed++; break;
            }
        }
    }
}
