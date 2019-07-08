using System.Collections.ObjectModel;
using Worktile.Main;
using Worktile.Models;
using Worktile.Common;
using Windows.UI.Xaml.Controls;

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

        public Frame Frame { get; set; }

        public void Highlight(Session session)
        {
            _session = session;
            OnPropertyChanged(nameof(Session));
        }
    }
}
