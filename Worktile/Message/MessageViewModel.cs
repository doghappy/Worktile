using System.Collections.ObjectModel;
using Worktile.Main;
using Worktile.Models;
using Worktile.Common;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.Linq;

namespace Worktile.Message
{
    public class MessageViewModel : BindableBase
    {
        public ObservableCollection<Session> Sessions => MainViewModel.Sessions;

        private Session _session;
        public Session Session
        {
            get => _session;
            set
            {
                if (SetProperty(ref _session, value) && value != null)
                {
                    Frame.Navigate(typeof(MessageDetailPage), value);
                }
            }
        }

        private Session _rightTappedSession;
        public Session RightTappedSession
        {
            get => _rightTappedSession;
            set => SetProperty(ref _rightTappedSession, value);
        }


        public Frame Frame { get; set; }

        private string RightTappedSessionType => RightTappedSession.Type.ToString().ToLower() + "s";

        public void Highlight(Session session)
        {
            _session = session;
            OnPropertyChanged(nameof(Session));
        }

        public async Task StarAsync()
        {
            string url = $"api/{RightTappedSessionType}/{RightTappedSession.Id}/star";
            var data = await WtHttpClient.PutAsync(url);
            if (data.Value<int>("code") == 200 && data.Value<bool>("data"))
            {
                RightTappedSession.IsStar = true;
            }
        }

        public async Task UnStarAsync()
        {
            string url = $"api/{RightTappedSessionType}/{RightTappedSession.Id}/unstar";
            var data = await WtHttpClient.PutAsync(url);
            if (data.Value<int>("code") == 200 && data.Value<bool>("data"))
            {
                RightTappedSession.IsStar = false;
            }
        }

        public async Task DeleteRightTappedSessionAsync()
        {
            if (Sessions.Count > 1)
            {
                bool success;
                if (RightTappedSession.Type == SessionType.Channel)
                {
                    string url = $"api/channels/{RightTappedSession.Id}/disable";
                    var data = await WtHttpClient.PutAsync(url);
                    success = data.Value<int>("code") == 200;
                }
                else
                {
                    string url = $"api/sessions/{RightTappedSession.Id}";
                    var data = await WtHttpClient.DeleteAsync(url);
                    success = data.Value<int>("code") == 200 && data.Value<bool>("data");
                }
                if (success)
                {
                    Sessions.Remove(RightTappedSession);
                    Session = Sessions.First();
                }
            }
        }
    }
}
