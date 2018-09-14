using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Worktile.WindowsUI.Enums.Mission;

namespace Worktile.WindowsUI.Converters.Mission
{
    class SubHeaderForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = Enum.Parse<MissionActivityStatus>(value.ToString());
            var param = Enum.Parse<MissionActivityStatus>(parameter.ToString());
            if (status == param)
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
