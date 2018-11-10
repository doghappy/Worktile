using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModel.ApiTeam;
using Worktile.ApiModel.ApiUserMe;
using Worktile.Models;
using Worktile.Services;
using Worktile.WtRequestClient;

namespace Worktile
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            AppItems = new ObservableCollection<WtApp>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WtApp> AppItems { get; }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RequestApiUserMeAsync();
            await RequestApiTeamAsync();
        }

        private async Task RequestApiUserMeAsync()
        {
            string uri = CommonData.SubDomain + "/api/user/me";
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>(uri);
            CommonData.ApiUserMeConfig = me.Data.Config;
        }

        private async Task RequestApiTeamAsync()
        {
            string uri = CommonData.SubDomain + "/api/team";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>(uri);
            CommonData.Team = data.Data;
            CommonData.Team.Apps.ForEach(app =>
            {
                var item = CommonData.Apps.SingleOrDefault(a => a.Name == app.Name);
                if (item != null)
                {
                    AppItems.Add(item);
                }
            });
        }
    }
}
