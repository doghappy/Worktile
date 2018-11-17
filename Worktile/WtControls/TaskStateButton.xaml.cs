using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModel.ApiMissionVnextTasksIdstates;
using Worktile.ApiModel.ApiMissionVnextWorkMyDirectedActive;
using Worktile.Common;
using Worktile.WtRequestClient;

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

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            IsActive = true;
            string uri = $"/api/mission-vnext/tasks/{TaskId}/states";
            var clicent = new WtHttpClient();
            var data = await clicent.GetAsync<ApiMissionVnextTasksIdstates>(uri);
            AllStates = data.Data.Value.Select(i => new Models.TaskState
            {
                Foreground = WtColorHelper.GetNewColor(i.Color),
                Name = i.Name,
                Glyph = WtIconHelper.GetGlyph(i.Type)
            })
            .ToList();
            IsActive = false;
        }
    }
}
