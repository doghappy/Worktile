using System;
using Windows.UI.Xaml.Data;
using Worktile.Common;

namespace Worktile.Converters
{
    public class AvatarBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return WtColorHelper.GetAvatarBackground(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
