using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Worktile.WindowsUI.Converters.Mission
{
    class SubHeaderForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = int.Parse(value.ToString());
            int param = int.Parse(parameter.ToString());
            if (index == param)
            {
                var color = (Color)Application.Current.Resources["SystemAccentColor"];
                return new SolidColorBrush(color);
            }
            return Application.Current.Resources["SystemColorGrayTextBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
