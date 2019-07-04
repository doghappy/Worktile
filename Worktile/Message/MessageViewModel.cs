using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Main;
using Worktile.Models;
using Worktile.Common;

namespace Worktile.Message
{
    public class MessageViewModel : BindableBase
    {
        public ObservableCollection<Session> Sessions => MainViewModel.Sessions;

        private Session _session;
        public Session Session
        {
            get => _session;
            set => SetProperty(ref _session, value);
        }
    }
}
