using System;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double val = double.Parse(value.ToString());
            if (parameter == null)
            {
                return val.ToString("p");
            }
            else
            {
                return val.ToString(parameter.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
