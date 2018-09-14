using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Enums.Mission;
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
                    ChangeFrame();
                }
            }
        }

        private MissionActivityStatus activityStatus;
        public MissionActivityStatus ActivityStatus
        {
            get => activityStatus;
            set
            {
                if (activityStatus != value)
                {
                    activityStatus = value;
                    OnPropertyChanged();
                    ChangeFrame();
                }
            }
        }

        private Frame Frame
        {
            get
            {
                var fe = Window.Current.Content as FrameworkElement;
                return fe.GetChild<Frame>("MissionMainFrame");
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeFrame()
        {
            int index = WorkAddon.Features.IndexOf(SelectedFeature);
            if (index == 0 && ActivityStatus == MissionActivityStatus.Active)
            {
                //看板
            }
            else
            {
                //Table
            }
        }
    }
}
