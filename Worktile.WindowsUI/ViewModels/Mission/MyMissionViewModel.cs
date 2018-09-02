using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.WindowsUI.Models.General;
using Worktile.WindowsUI.Models.Mission;

namespace Worktile.WindowsUI.ViewModels.Mission
{
    public class MyMissionViewModel : MainAreaViewModel, INotifyPropertyChanged
    {
        public MyMissionViewModel(WorkAddon workAddon)
        {
            WorkAddon = workAddon;
            SelectedFeature = WorkAddon.Features.FirstOrDefault();
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public WorkAddon WorkAddon { get; }

        private KeyName selectedFeature;
        public KeyName SelectedFeature
        {
            get => selectedFeature;
            set
            {
                if (selectedFeature != value)
                {
                    selectedFeature = value;
                    OnPropertyChanged();
                }
            }
        }

        private int subHeaderSelectedIndex;
        public int SubHeaderSelectedIndex
        {
            get => subHeaderSelectedIndex;
            set
            {
                if (subHeaderSelectedIndex != value)
                {
                    subHeaderSelectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
