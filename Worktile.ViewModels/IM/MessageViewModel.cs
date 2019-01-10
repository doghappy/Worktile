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
    public abstract class MessageViewModel
    {
        protected abstract void OnPropertyChanged([CallerMemberName]string prop = null);

        public abstract ObservableCollection<ChatNavItem> NavItems { get; }

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
    }
}
