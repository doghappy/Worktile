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
    public class GeneralMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public GeneralMessageViewModel()
        {
            NavItems = new ObservableCollection<ChatNavItem>
            {
                new ChatNavItem { Key = "unread", Name = "消息" },
                new ChatNavItem { Key = "read", Name = "文件" },
                new ChatNavItem { Key = "pending", Name = "固定消息" }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<ChatNavItem> NavItems { get; }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
