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
        }

        public async Task UnStarAsync()
        {
        }

        public async Task DeleteRightTappedSessionAsync()
        {
            
        }
    }
}
