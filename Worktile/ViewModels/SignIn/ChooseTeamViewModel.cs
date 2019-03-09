using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common.WtRequestClient;
using Worktile.Models.Team;
using Worktile.Views;

namespace Worktile.ViewModels.SignIn
{
    class ChooseTeamViewModel : ViewModel, INotifyPropertyChanged
    {
        public ChooseTeamViewModel(Frame frame, string passToken)
        {
            _frame = frame;
            _passToken = passToken;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly private Frame _frame;
        readonly private string _passToken;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task SignInAsync(Team team)
        {
            IsActive = true;
            string url = "https://worktile.com/api/account/user/signin";
            var req = new
            {
                pass_token = _passToken,
                team_id = team.Id
            };
            var res = await WtHttpClient.PostAsync<ApiResponse>(url, req);
            if (res.Code == 200)
            {
                IsActive = false;
                _frame.Navigate(typeof(MainPage));
                WtHttpClient.Domain = team.Domain;
                ApplicationData.Current.LocalSettings.Values["Domain"] = team.Domain;
            }
        }
    }
}
