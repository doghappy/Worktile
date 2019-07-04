using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Main;
using Worktile.Models;

namespace Worktile.Message
{
    public class MessageViewModel
    {
        public ObservableCollection<Session> Sessions => MainViewModel.Sessions;
    }
}
