using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using WtMessage = Worktile.Message.Models;
using Windows.UI.Xaml;

namespace Worktile.Controls.Message
{
    public sealed partial class ImageMessage : UserControl, INotifyPropertyChanged
    {
        public ImageMessage()
        {
            InitializeComponent();
            IsButtonEnabled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<WtMessage.Message> OnImageMessageClick;

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsButtonEnabled)));
                }
            }
        }


        private WtMessage.Message _message;
        public WtMessage.Message Message
        {
            get => _message;
            set
            {
                if (_message != value && value.Type == WtMessage.MessageType.Image)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnImageMessageClick?.Invoke(Message);
        }
    }
}
