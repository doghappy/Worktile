using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common.WtRequestClient;
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

        public async Task SignInAsync(string teamId)
        {
            IsActive = true;
            string url = "https://worktile.com/api/account/user/signin";
            var req = new
            {
                pass_token = _passToken,
                team_id = teamId
            };
            var client = new WtHttpClient();
            client.OnSuccessStatusCode += resMsg =>
            {
                string cookieString = resMsg.Headers.GetValues("Set-Cookie").First();
                var cookie = WtHttpClient.GetCookieByString(cookieString);
                cookie.Expires = cookie.Expires.AddYears(20);
                cookieString = WtHttpClient.GetStringByCookie(cookie);
                ApplicationData.Current.LocalSettings.Values["AuthCookie"] = cookieString;
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookieString);
            };
            var res = await client.PostAsync<ApiResponse>(url, req);
            IsActive = false;
            _frame.Navigate(typeof(MainPage));
        }
    }
}
