﻿using System.ComponentModel;
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
                }
            }
        }
    }
}
