using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    class ChannelMessageViewModel : SessionMessageViewModel<ChannelSession>, INotifyPropertyChanged
    {
        public ChannelMessageViewModel(ChannelSession session) : base(session) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override string IdType => "channel_id";

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
