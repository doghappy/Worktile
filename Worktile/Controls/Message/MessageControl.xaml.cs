using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;

namespace Worktile.Controls
{
    public sealed partial class MessageControl : UserControl
    {
        public MessageControl()
        {
            InitializeComponent();
        }

        public ViewMessage Message
        {
            get { return (ViewMessage)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(ViewMessage), typeof(MessageControl), new PropertyMetadata(default(ViewMessage)));

        public object RightContent
        {
            get { return GetValue(RightContentProperty); }
            set { SetValue(RightContentProperty, value); }
        }
        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register("RightContent", typeof(object), typeof(MessageControl), new PropertyMetadata(null));
    }
}
