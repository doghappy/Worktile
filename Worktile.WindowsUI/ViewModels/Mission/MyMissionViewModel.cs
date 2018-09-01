using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.WindowsUI.Models.Mission;

namespace Worktile.WindowsUI.ViewModels.Mission
{
    public class MyMissionViewModel : ViewModel
    {
        public MyMissionViewModel(WorkAddon workAddon)
        {
            WorkAddon = workAddon;
        }

        public WorkAddon WorkAddon { get; }
    }
}
