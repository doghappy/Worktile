using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.IM.Message;

namespace Worktile.WtControls.IM
{
    public sealed partial class MissionVNextMessage : UserControl, INotifyPropertyChanged
    {
        public MissionVNextMessage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Message _message;
        public Message Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
    }
}
