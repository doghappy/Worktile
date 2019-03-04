using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content.Pin
{
    class MemberPinnedViewModel : PinnedViewModel<MemberSession>, INotifyPropertyChanged
    {
        public MemberPinnedViewModel(MemberSession session) : base(session) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override string IdType => "session_id";

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
