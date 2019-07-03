using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.SignInOut.Models;

namespace Worktile.Main
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            Apps = new ObservableCollection<WtApp>
            {
                new WtApp
                {
                    Name = "message",
                    DisplayName = UtilityTool.GetStringFromResources("WtAppMessageDisplayName"),
                    Icon = WtIconHelper.GetAppIcon("message")
                }
            };
            SelectedApp = Apps.First();
            UserProfileText = UtilityTool.GetStringFromResources("SignIn");
        }

        public ObservableCollection<WtApp> Apps { get; }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set => SetProperty(ref _selectedApp, value);
        }

        private string _userProfileText;
        public string UserProfileText
        {
            get => _userProfileText;
            set => SetProperty(ref _userProfileText, value);
        }

        private Worktile.Models.User _user;
        public Worktile.Models.User User
        {
            get => _user;
            set
            {
                SetProperty(ref _user, value);
                if (value == null)
                {
                    UserProfileText = UtilityTool.GetStringFromResources("SignIn");
                }
                else
                {
                    UserProfileText = value.DisplayName;
                }
            }
        }
    }
}
