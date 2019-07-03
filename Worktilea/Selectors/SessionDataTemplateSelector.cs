using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message.Session;

namespace Worktile.Selectors
{
    class SessionDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChannelTemplate { get; set; }
        public DataTemplate MemberTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item.GetType() == typeof(ChannelSession))
            {
                return ChannelTemplate;
            }
            else
            {
                return MemberTemplate;
            }
        }
    }
}
