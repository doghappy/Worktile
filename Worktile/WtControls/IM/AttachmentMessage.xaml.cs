using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.IM.Message;

namespace Worktile.WtControls.IM
{
    public sealed partial class AttachmentMessage : UserControl, INotifyPropertyChanged
    {
        public AttachmentMessage()
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



        //public Message Message
        //{
        //    get { return (Message)GetValue(MessageProperty); }
        //    set { SetValue(MessageProperty, value); }
        //}
        //public static readonly DependencyProperty MessageProperty =
        //    DependencyProperty.Register("Message", typeof(Message), typeof(AttachmentMessage), new PropertyMetadata(null));
    }
}
