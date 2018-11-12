using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModel.ApiMissionVnextTasksIdstates;
using Worktile.Models.KanBan;
using Worktile.WtRequestClient;
using System.Linq;
using Worktile.Services;

namespace Worktile.WtControls.Mission
{
    public sealed partial class MissionListViewItem : UserControl, INotifyPropertyChanged
    {
        public MissionListViewItem()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private KanBanItem _kanBanItem;
        public KanBanItem KanBanItem
        {
            get => _kanBanItem;
            set
            {
                _kanBanItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KanBanItem)));
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



        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (KanBanItem.AllStates == null)
            {
                IsActive = true;
                string uri = $"/api/mission-vnext/tasks/{KanBanItem.Id}/states";
                var clicent = new WtHttpClient();
                var data = await clicent.GetAsync<ApiMissionVnextTasksIdstates>(uri);
                KanBanItem.AllStates = data.Data.Value.Select(i => new KanBanItemState
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
}
