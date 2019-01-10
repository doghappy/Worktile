using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels.IM.ApiPigeonMessages;
using Worktile.Common;
using Worktile.Models.IM;
using Worktile.WtRequestClient;

namespace Worktile.ViewModels.IM
{
    public abstract class MessageViewModel
    {
        protected abstract void OnPropertyChanged([CallerMemberName]string prop = null);
        public abstract ObservableCollection<ChatNavItem> NavItems { get; }
        protected abstract string MessageUrl { get; }

        private ChatSession _session;
        public ChatSession Session
        {
            get => _session;
            set
            {
                if (_session != value)
                {
                    _session = value;
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
            var data = await client.GetAsync<ApiPigeonMessages>(MessageUrl);
            SelectedNav.HasMore = data.Data.Next != null;
            SelectedNav.Next = data.Data.Next;
            //data.Data.Messages.Reverse();
            foreach (var item in data.Data.Messages)
            {
                item.From.Avatar = AvatarHelper.GetAvatarUrl(item.From.Avatar, 80);
                item.From.Background = AvatarHelper.GetColor(item.From.DisplayName);
                item.From.Initials = AvatarHelper.GetInitials(item.From.DisplayName);
                SelectedNav.Messages.Insert(0, item);
            }
            IsActive = false;
        }
    }
}
