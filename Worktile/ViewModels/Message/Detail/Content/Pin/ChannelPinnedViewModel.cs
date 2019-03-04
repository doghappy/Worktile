using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content.Pin
{
    class ChannelPinnedViewModel : PinnedViewModel<ChannelSession>, INotifyPropertyChanged
    {
        public ChannelPinnedViewModel(ChannelSession session) : base(session) { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override string IdType => "channel_id";

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
