using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.Models.IM;
using Worktile.WtRequestClient;

namespace Worktile.ViewModels.IM
{
    public abstract class MessageViewModel
    {
        public MessageViewModel()
        {
            NavItems = new ObservableCollection<ChatNavItem>();
        }

        public ObservableCollection<ChatNavItem> NavItems { get; }

        protected string Url
        {
            get
            {
                int refType = Session.Type == ChatType.Channel ? 1 : 2;
                if (Session.IsAssistant)
                {
                    string component = null;
                    if (Session.Component.HasValue)
                    {
                        component = GetComponentByNumber(Session.Component.Value);
                    }
                    string url = $"/api/pigeon/messages?ref_id={Session.Id}&ref_type={refType}&filter_type={SelectedNav.FilterType}&component={component}&size=20";
                    if (!string.IsNullOrEmpty(SelectedNav.Next))
                    {
                        url += "&next=" + SelectedNav.Next;
                    }
                    return url;
                }
                else
                {
                    string url = "/api/messages?";
                    if (SelectedNav.IsPin)
                    {
                        url += $"session_id={Session.Id}&size=20";
                    }
                    else
                    {
                        url += $"ref_id={Session.Id}&ref_type={refType}&latest_id={SelectedNav.LatestId}&size=20";
                    }
                    return url;
                }
            }
        }
        protected abstract void OnPropertyChanged([CallerMemberName]string prop = null);
        protected abstract void ReadApiData(JToken jToken);

        private ChatSession _session;
        public ChatSession Session
        {
            get => _session;
            set
            {
                if (_session != value)
                {
                    _session = value;
                    if (value.IsAssistant)
                    {
                        NavItems.Add(new ChatNavItem { Name = "未读", FilterType = 2 });
                        NavItems.Add(new ChatNavItem { Name = "已读", FilterType = 4 });
                        NavItems.Add(new ChatNavItem { Name = "待处理", FilterType = 3 });
                    }
                    else
                    {
                        NavItems.Add(new ChatNavItem { Name = "消息" });
                        NavItems.Add(new ChatNavItem { Name = "文件" });
                        NavItems.Add(new ChatNavItem { Name = "固定消息", IsPin = true });
                    }
                    SelectedNav = NavItems.First();
                    OnPropertyChanged();
                }
            }
        }

        private ChatNavItem _selectedNav;
        public ChatNavItem SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task LoadMessagesAsync()
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetJTokenAsync(Url);
            ReadApiData(data);
            IsActive = false;
            SelectedNav.EmptyFrameVisible = !SelectedNav.Messages.Any();
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

        public static MessageViewModel GetViewModel(ChatSession session)
        {
            MessageViewModel viewModel = null;
            if (session.IsAssistant)
            {
                viewModel = new AssistantMessageViewModel();
            }
            else
            {
                viewModel = new SessionMessageViewModel();
            }
            viewModel.Session = session;
            return viewModel;
        }
    }
}
