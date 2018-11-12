using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModel.ApiMissionVnextWorkMyDirectedActive;
using Worktile.Models;
using Worktile.Models.KanBan;
using Worktile.Services;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.My
{
    public sealed partial class MyDirectActivePage : Page, INotifyPropertyChanged
    {
        public MyDirectActivePage()
        {
            InitializeComponent();
            KanBanGroups = new ObservableCollection<KanBanGroup>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private bool _isPageLoaed;

        public ObservableCollection<KanBanGroup> KanBanGroups { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestApiMissionVnextWorkMyDirectedActive();
            IsActive = false;
            _isPageLoaed = true;
        }

        private async Task RequestApiMissionVnextWorkMyDirectedActive()
        {
            string uri = "/api/mission-vnext/work/my/directed/active";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkMyDirectedActive>(uri);

            foreach (var item in data.Data.References.Groups)
            {
                var kbGroup = new KanBanGroup
                {
                    Header = item.Name
                };
                foreach (var taskId in item.TaskIds)
                {
                    var task = data.Data.Value.Single(v => v.Id == taskId);
                    var state = data.Data.References.Lookups.TaskStates.Single(t => t.Id == task.TaskStateId);
                    var type = data.Data.References.TaskTypes.Single(t => t.Id == task.TaskTypeId);
                    switch (state.Type)
                    {
                        case 1: kbGroup.NotStarted++; break;
                        case 2: kbGroup.Processing++; break;
                        case 3: kbGroup.Completed++; break;
                    }
                    kbGroup.Items.Add(new KanBanItem
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Identifier = task.Identifier,
                        Avatar = new Avatar
                        {
                            ProfilePicture = CommonData.GetAvatarUrl(CommonData.ApiUserMe.Avatar, 40),
                            DisplayName = CommonData.ApiUserMe.DisplayName
                        },
                        State = new Models.TaskState
                        {
                            Name = state.Name,
                            Foreground = WtColorHelper.GetNewColor(state.Color),
                            Glyph = WtIconHelper.GetGlyph(state.Type)
                        },
                        TaskType = new Models.TaskType
                        {
                            Name = type.Name,
                            Color = WtColorHelper.GetColorByClass(type.Icon),
                            Glyph = WtIconHelper.GetGlyph("wtf-type-" + type.Icon),
                        }
                    });
                }
                KanBanGroups.Add(kbGroup);
            }
        }

        private async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var sp = MyGrid.GetChild<StackPanel>("MissionHeader");
            var lvs = MyGrid.GetChildren<ListView>("DataList");
            if (sp != null && lvs.Any())
            {
                foreach (var item in lvs)
                {
                    item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                }
            }
            else
            {
                for (int i = 0; i < 200; i++)
                {
                    if (_isPageLoaed)
                    {
                        sp = MyGrid.GetChild<StackPanel>("MissionHeader");
                        lvs = MyGrid.GetChildren<ListView>("DataList");
                        if (sp != null && lvs.Any())
                        {
                            foreach (var item in lvs)
                            {
                                item.MaxHeight = MyGrid.ActualHeight - 24 - sp.ActualHeight;
                            }
                            return;
                        }
                    }
                    await Task.Delay(100);
                }
            }
        }
    }
}
