using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Worktile.Controls.Message
{
    public sealed partial class AttachmentMessage : UserControl, INotifyPropertyChanged
    {
        public AttachmentMessage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Models.Message.Message _message;
        public Models.Message.Message Message
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
