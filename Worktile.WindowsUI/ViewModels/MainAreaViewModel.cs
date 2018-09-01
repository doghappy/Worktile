using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.WindowsUI.Common;

namespace Worktile.WindowsUI.ViewModels
{
    public abstract class MainAreaViewModel : ViewModel
    {
        public MainAreaViewModel()
        {

        }

        public string BackgroundImage { get; }
    }
}
