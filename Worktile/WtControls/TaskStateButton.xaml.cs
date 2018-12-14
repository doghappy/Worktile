using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Worktile.WtControls
{
    public sealed partial class TaskStateButton : UserControl, INotifyPropertyChanged
    {
        public TaskStateButton()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Models.TaskState _state;
        public Models.TaskState State
        {
            get => _state;
            set
            {
                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }


        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
            }
        }

        private string _taskId;
        public string TaskId
        {
            get => _taskId;
            set
            {
                _taskId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskId)));
            }
        }

        private List<Models.TaskState> _allStates;
        public List<Models.TaskState> AllStates
        {
            get => _allStates;
            set
            {
                _allStates = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllStates)));
            }
        }

        private async void Flyout_Opened(object sender, object e)
        {
            await StateList.OpenAsync();
        }
    }
}
