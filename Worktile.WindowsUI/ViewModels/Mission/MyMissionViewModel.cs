using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.WindowsUI.Models.Mission;

namespace Worktile.WindowsUI.ViewModels.Mission
{
    public class MyMissionViewModel : MainAreaViewModel, INotifyPropertyChanged
    {
        public MyMissionViewModel(WorkAddon workAddon)
        {
            WorkAddon = workAddon;
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public WorkAddon WorkAddon { get; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
