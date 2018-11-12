using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.KanBan;

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
    }
}
