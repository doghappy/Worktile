using System;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    public class StrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return "-";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
