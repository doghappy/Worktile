using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels.IM.ApiPigeonMessages;
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
        protected abstract string MessageUrl { get; }
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
                        NavItems.Add(new ChatNavItem { Key = "unread", Name = "未读", FilterType = 2 });
                        NavItems.Add(new ChatNavItem { Key = "read", Name = "已读", FilterType = 4 });
                        NavItems.Add(new ChatNavItem { Key = "pending", Name = "待处理", FilterType = 3 });
                    }
                    else
                    {
                        NavItems.Add(new ChatNavItem { Key = "unread", Name = "消息" });
                        NavItems.Add(new ChatNavItem { Key = "read", Name = "文件" });
                        NavItems.Add(new ChatNavItem { Key = "pending", Name = "固定消息" });
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
            var data = await client.GetJTokenAsync(MessageUrl);
            ReadApiData(data);
            //var data = await client.GetAsync<ApiPigeonMessages>(MessageUrl);
            //SelectedNav.HasMore = data.Data.Next != null;
            //OnMessageResponsed(data);
            //foreach (var item in data.Data.Messages)
            //{
            //    item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, 80);
            //    item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
            //    item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);
            //    SelectedNav.Messages.Insert(0, item);
            //}
            IsActive = false;
        }

        public async Task ScrollViewerChangedAsync(ScrollViewer scrollViewer)
        {
            if (scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset <= 10)
                SelectedNav.ScrollMode = ItemsUpdatingScrollMode.KeepLastItemInView;
            else
                SelectedNav.ScrollMode = ItemsUpdatingScrollMode.KeepItemsInView;

            if (SelectedNav.HasMore.HasValue && SelectedNav.HasMore.Value
                && !IsActive && scrollViewer.VerticalOffset <= 10)
            {
                await LoadMessagesAsync();
            }
        }
    }
}
