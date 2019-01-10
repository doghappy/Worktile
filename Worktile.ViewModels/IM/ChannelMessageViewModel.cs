using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.Models.IM;

namespace Worktile.ViewModels.IM
{
    public class ChannelMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public ChannelMessageViewModel()
        {
            NavItems = new ObservableCollection<ChatNavItem>
            {
                new ChatNavItem { Key = "unread", Name = "未读" },
                new ChatNavItem { Key = "read", Name = "已读" },
                new ChatNavItem { Key = "pending", Name = "待处理" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<ChatNavItem> NavItems { get; }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
