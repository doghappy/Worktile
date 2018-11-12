using System.Collections.Generic;
using System.ComponentModel;

namespace Worktile.Models.KanBan
{
    public class KanBanItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public KanBanItemAvatar Avatar { get; set; }
        public KanBanItemState State { get; set; }
        public string Title { get; set; }
        public string Identifier { get; set; }
        public KanBanItemTaskType TaskType { get; set; }

        private List<KanBanItemState> _allStates;
        public List<KanBanItemState> AllStates
        {
            get => _allStates;
            set
            {
                _allStates = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllStates)));
            }
        }
    }
}
