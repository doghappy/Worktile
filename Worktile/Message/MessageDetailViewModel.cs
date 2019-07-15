using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Main;
using Worktile.Message.Models;
using Worktile.Models;

namespace Worktile.Message
{
    public class MessageDetailViewModel : BindableBase
    {
        public MessageDetailViewModel()
        {
            Navs = new ObservableCollection<MessageNav>();
        }

        public Session Session { get; set; }

        public ObservableCollection<MessageNav> Navs { get; }

        private MessageNav _nav;
        public MessageNav Nav
        {
            get => _nav;
            set => SetProperty(ref _nav, value);
        }

        public void LoadNavs()
        {
            if (Session.IsAAssistant)
            {
                string unread = UtilityTool.GetStringFromResources("MessageDetailPageNavUnread");
                string read = UtilityTool.GetStringFromResources("MessageDetailPageNavRead");
                string later = UtilityTool.GetStringFromResources("MessageDetailPageNavLater");
                Navs.Add(new MessageNav { Tag = "Unread", Content = unread });
                Navs.Add(new MessageNav { Tag = "Read", Content = read });
                Navs.Add(new MessageNav { Tag = "Later", Content = later });
            }
            else
            {
                string message = UtilityTool.GetStringFromResources("MessageDetailPageNavMessage");
                string file = UtilityTool.GetStringFromResources("MessageDetailPageNavFiles");
                string pin = UtilityTool.GetStringFromResources("MessageDetailPageNavPinnedMessages");
                Navs.Add(new MessageNav { Tag = "Message", Content = message });
                Navs.Add(new MessageNav { Tag = "File", Content = file });
                Navs.Add(new MessageNav { Tag = "Pin", Content = pin });
            }
            Nav = Navs.First();
        }

        public async Task ClearUnreadAsync()
        {
            string url = $"api/messages/unread/clear?ref_id={Session.Id}";
            var obj = await WtHttpClient.PutAsync(url);
            if (obj.Value<int>("code") == 200)
            {
                MainViewModel.UnreadMessageCount -= Session.UnRead;
                Session.UnRead = 0;
            }
        }
    }
}
