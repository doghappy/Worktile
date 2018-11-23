using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Worktile.Common;
using Worktile.Models.Kanban;

namespace Worktile.WtControls.Mission
{
    public sealed partial class MissionListViewItem : UserControl, INotifyPropertyChanged
    {
        public MissionListViewItem()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private KanbanItem _kanbanItem;
        public KanbanItem KanbanItem
        {
            get => _kanbanItem;
            set
            {
                _kanbanItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KanbanItem)));
            }
        }
    }
}
