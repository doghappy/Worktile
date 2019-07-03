using System.ComponentModel;
using System.IO;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Enums.Message;

namespace Worktile.Controls.Message
{
    public sealed partial class DriveMessage : UserControl, INotifyPropertyChanged
    {
        public DriveMessage()
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
                if (_message != value && value.Type == MessageType.Drive)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Models.Message.Message)));
                    Icon = WtFileHelper.GetFileIcon(Path.GetFileNameWithoutExtension(value.Body.InlineAttachment.Img));
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
