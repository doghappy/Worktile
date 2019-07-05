using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Controls.Message
{
    public sealed partial class FileMessage : UserControl, INotifyPropertyChanged
    {
        public FileMessage()
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
                if (_message != value && value.Type == WtMessage.MessageType.File)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                    //Icon = WtFileHelper.GetFileIcon(value.Body.Attachment.Addition.Ext);
                }
            }
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Icon)));
                }
            }
        }
    }
}
