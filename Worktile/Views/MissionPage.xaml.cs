using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextProjectNav;
using Worktile.WtRequestClient;

namespace Worktile.Views
{
    public sealed partial class MissionPage : Page
    {
        public MissionPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RequestApiMissionVnextProjectNav();
        }

        private async Task RequestApiMissionVnextProjectNav()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextProjectNav>("/api/mission-vnext/project-nav");
        }
    }
}
