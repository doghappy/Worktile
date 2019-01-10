using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.Models.IM;
using Worktile.Models.IM.Message;

namespace Worktile.ViewModels.IM
{
    public class ChannelMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public ChannelMessageViewModel()
        {
            NavItems = new ObservableCollection<ChatNavItem>
            {
                new ChatNavItem { Key = "unread", Name = "未读", FilterType = 2 },
                new ChatNavItem { Key = "read", Name = "已读", FilterType = 4 },
                new ChatNavItem { Key = "pending", Name = "待处理", FilterType = 3 }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<ChatNavItem> NavItems { get; }

        protected override string MessageUrl
        {
            get
            {
                int refType = Session.IsBot ? 2 : 1;
                string component = null;
                if (Session.Component.HasValue)
                {
                    component = GetComponentByNumber(Session.Component.Value);
                }
                string uri = $"/api/pigeon/messages?ref_id={Session.Id}&ref_type={refType}&filter_type={SelectedNav.FilterType}&component={component}&size=20";
                if (!string.IsNullOrEmpty(SelectedNav.Next))
                {
                    uri += "&next=" + SelectedNav.Next;
                }
                return uri;
            }
        }

        private string GetComponentByNumber(int c)
        {
            string component = null;
            switch (c)
            {
                case 0: component = "message"; break;
                case 1: component = "drive"; break;
                case 2: component = "task"; break;
                case 3: component = "calendar"; break;
                case 4: component = "report"; break;
                case 5: component = "crm"; break;
                case 6: component = "approval"; break;
                case 9: component = "leave"; break;
                case 20: component = "bulletin"; break;
                case 30: component = "appraisal"; break;
                case 40: component = "okr"; break;
                case 50: component = "portal"; break;
                case 60: component = "mission"; break;
            }
            return component;
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
