using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class ChannelMessageViewModel : SessionMessageViewModel<ChannelSession>, INotifyPropertyChanged
    {
        public ChannelMessageViewModel(ChannelSession session, MainViewModel mainViewModel) : base(session, mainViewModel) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override string IdType => "channel_id";

        protected override int ToType => 1;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
