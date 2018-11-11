using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextWorkMyDirectedActive;
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

        public ObservableCollection<KanBanGroup> KanBanGroups { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestApiMissionVnextWorkMyDirectedActive();
            IsActive = false;
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
                        Title = task.Title,
                        Identifier = task.Identifier,
                        Avatar = new KanBanItemAvatar
                        {
                            ProfilePicture = CommonData.ApiUserMeConfig.Box.AvatarUrl + CommonData.ApiUserMe.Avatar,
                            DisplayName = CommonData.ApiUserMe.DisplayName
                        },
                        State = new KanBanItemState
                        {
                            Name = state.Name,
                            Foreground = WtColorHelper.GetNewColor(state.Color),
                            Glyph = WtIconHelper.GetGlyph(state.Type)
                        },
                        TaskType = new KanBanItemTaskType
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
    }
}
