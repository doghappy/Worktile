using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Controls.Message
{
    public sealed partial class CrmMessage : UserControl, INotifyPropertyChanged
    {
        public CrmMessage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private WtMessage.Message _message;
        public WtMessage.Message Message
        {
            get => _message;
            set
            {
                if (_message != value && (value.Type == WtMessage.MessageType.Crm || value.Type == WtMessage.MessageType.CrmContract))
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
    }
}
