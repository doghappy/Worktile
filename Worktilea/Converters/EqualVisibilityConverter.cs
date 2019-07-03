using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    public class EqualVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value?.ToString() == parameter.ToString())
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
