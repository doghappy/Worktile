using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextTask;

namespace Worktile.Views.Mission.Project.Detail
{
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private Data _task;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _task = e.Parameter as Data;
        }
    }
}
