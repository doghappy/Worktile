﻿using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Enums.Message;

namespace Worktile.Controls.Message
{
    public sealed partial class ReportMessage : UserControl, INotifyPropertyChanged
    {
        public ReportMessage()
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
                if (_message != value && value.Type == MessageType.Report)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Models.Message.Message)));
                }
            }
        }


    }
}
