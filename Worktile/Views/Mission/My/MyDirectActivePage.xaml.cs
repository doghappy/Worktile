using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels.ApiMissionVnextWorkMyDirectedActive;
using Worktile.Models.Kanban;
using Worktile.WtRequestClient;
using System.Collections.Generic;
using System;
using Worktile.ViewModels.Infrastructure;
using Worktile.Infrastructure;
using Worktile.Enums;

namespace Worktile.Views.Mission.My
{
    public sealed partial class MyDirectActivePage : Page, INotifyPropertyChanged
    {
        public MyDirectActivePage()
        {
            InitializeComponent();
            KanbanGroups = new ObservableCollection<KanbanGroup>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ObservableCollection<KanbanGroup> KanbanGroups { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestIndexDataAsync();
            IsActive = false;
        }

        private async Task RequestIndexDataAsync()
        {
            string uri = "/api/mission-vnext/work/my/directed/active";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkMyDirectedActive>(uri);

            foreach (var item in data.Data.References.Groups)
            {
                var kbGroup = new KanbanGroup
                {
                    Header = item.Name
                };
                foreach (var taskId in item.TaskIds)
                {
                    var task = data.Data.Value.Single(v => v.Id == taskId);
                    var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == task.TaskStateId);
                    var type = data.Data.References.TaskTypes.Single(t => t.Id == task.TaskTypeId);

                    KanbanPageHelper.ReadForProgressBar(kbGroup, state.Type);

                    var props = new List<KanbanItemProperty>
                    {
                        new KanbanItemProperty
                        {
                            Name = "任务编号",
                            Value = task.Identifier,
                            Foreground = WtColorHelper.GetSolidColorBrush("#aaaaaa"),
                            Background = WtColorHelper.GetSolidColorBrush("#1aaaaaaa")
                        }
                    };

                    var dueProperty = GetDueProperty(task.Properties.Due.Value.Date);
                    if (dueProperty != null)
                    {
                        props.Add(dueProperty);
                    }

                    kbGroup.Items.Add(new KanbanItem
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Properties = props,
                        Due = AvatarHelper.GetAvatar(DataSource.ApiUserMe.Uid, AvatarSize.X40),
                        State = new Models.TaskState
                        {
                            Name = state.Name,
                            Foreground = WtColorHelper.GetNewColor(state.Color),
                            BoldGlyph = WtIconHelper.GetBoldGlyph(state.Type)
                        },
                        TaskType = new Models.TaskType
                        {
                            Name = type.Name,
                            Color = WtColorHelper.GetColorByClass(type.Icon),
                            Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
                        }
                    });
                }
                KanbanGroups.Add(kbGroup);
            }
        }

        private KanbanItemProperty GetDueProperty(DateTime? dueDate)
        {
            if (dueDate.HasValue)
            {
                return new KanbanItemProperty
                {
                    Name = "截止时间",
                    Value = dueDate.Value.ToWtKanbanDate(),
                    Foreground = WtColorHelper.GetForegroundWithExpire(dueDate.Value),
                    Background = WtColorHelper.GetBackgroundWithExpire(dueDate.Value)
                };
            }
            return null;
        }

        private async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await KanbanPageHelper.KanbanGridAdaptiveAsync(MyGrid);
        }
    }
}
