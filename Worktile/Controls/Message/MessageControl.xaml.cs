using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Worktile.Controls
{
    public sealed partial class MessageControl : UserControl, INotifyPropertyChanged
    {
        public MessageControl()
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Models.Message.Message)));
                }
            }
        }

        public object RightContent
        {
            get { return GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }
        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(MessageControl), new PropertyMetadata(null));

        private void ActivityMsg_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            const string atPrefix = "worktile://at/";
            if (e.Link.StartsWith(atPrefix))
            {
                string uid = e.Link.Substring(atPrefix.Length);
            }
        }
    }
}
