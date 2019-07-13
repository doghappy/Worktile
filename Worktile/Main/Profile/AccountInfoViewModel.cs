using Worktile.Common;
using Worktile.Models;

namespace Worktile.Main.Profile
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
