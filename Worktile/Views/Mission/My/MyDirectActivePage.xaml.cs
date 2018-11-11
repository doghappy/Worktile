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
            MyGrid.AddHandler(PointerReleasedEvent, new PointerEventHandler(Grid_PointerReleased), true);
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

        private bool _isPressed;
        private double _x;

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

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _isPressed = true;
            _x = e.GetCurrentPoint(BigScrolViewer).Position.X;
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isPressed = false;
        }

        private void Grid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_isPressed)
            {
                double offset = BigScrolViewer.HorizontalOffset + (e.GetCurrentPoint(BigScrolViewer).Position.X - _x) / 4;
                BigScrolViewer.ChangeView(offset, null, null);
            }
        }

        private void MyGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isPressed = false;
        }
    }
}
