using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class MemberMessageViewModel : SessionMessageViewModel<MemberSession>, INotifyPropertyChanged
    {
        public MemberMessageViewModel(MemberSession session, MainViewModel mainViewModel) : base(session, mainViewModel) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override string IdType => "session_id";
        protected override int ToType => 2;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
