using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Worktile.Controls
{
    public sealed partial class MessageControl : UserControl
    {
        public MessageControl()
        {
            InitializeComponent();
        }

        public Models.Message.Message Message
        {
            get { return (Models.Message.Message)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(Models.Message.Message), typeof(MessageControl), new PropertyMetadata(default(Models.Message.Message)));

        public object RightContent
        {
            get { return GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }
        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(MessageControl), new PropertyMetadata(null));
    }
}
