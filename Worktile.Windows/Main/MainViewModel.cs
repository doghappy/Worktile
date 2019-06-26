using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Windows.Common;
using Worktile.Windows.Main.Models;

namespace Worktile.Windows.Main
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
        }

        public ObservableCollection<WtApp> Apps { get; }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get { return _selectedApp; }
            set { SetProperty(ref _selectedApp, value); }
        }
    }
}
