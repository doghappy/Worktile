using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels.ApiMissionVnextTasksIdstates;
using Worktile.Common;
using Worktile.Common.Communication;

namespace Worktile.WtControls.Flyout
{
    public sealed partial class TaskStateList : UserControl, INotifyPropertyChanged
    {
        public TaskStateList()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State), typeof(Models.TaskState), typeof(TaskStateList), null);
        public Models.TaskState State
        {
            get => GetValue(StateProperty) as Models.TaskState;
            set => SetValue(StateProperty, value);
        }


        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private string _taskId;
        public string TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId != value)
                {
                    _taskId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskId)));
                }
            }
        }

        private List<Models.TaskState> _allStates;
        public List<Models.TaskState> AllStates
        {
            get => _allStates;
            set
            {
                if (_allStates != value)
                {
                    _allStates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllStates)));
                }
            }
        }

        public async Task OpenAsync()
        {
            if (AllStates == null)
            {
                IsActive = true;
                string uri = $"/api/mission-vnext/tasks/{TaskId}/states";
                var data = await WtHttpClient.GetAsync<ApiMissionVnextTasksIdstates>(uri);
                AllStates = data.Data.Value.Select(i => new Models.TaskState
                {
                    Id = i.Id,
                    Foreground = WtColorHelper.GetNewColor(i.Color),
                    Name = i.Name,
                    BoldGlyph = WtIconHelper.GetBoldGlyph(i.Type),
                    LightGlyph = WtIconHelper.GetLightGlyph(i.Type)
                })
                .ToList();
                if (AllStates.Any())
                {
                    State = AllStates.Single(s => s.Id == State.Id);
                }
                IsActive = false;
            }
        }
    }
}
