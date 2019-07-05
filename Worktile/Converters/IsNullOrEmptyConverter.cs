using System;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    public class IsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
            {
                if (value == null)
                {
                    return true;
                }
                else
                {
                    return value.ToString() == string.Empty;
                }
            }
            else if (parameter.ToString() == "!")
            {
                if (value == null)
                {
                    return false;
                }
                else
                {
                    return value.ToString() != string.Empty;
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
