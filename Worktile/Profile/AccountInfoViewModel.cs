using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Models;

namespace Worktile.Profile
{
    public class AccountInfoViewModel : BindableBase
    {
        private User _user;
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
    }
}
