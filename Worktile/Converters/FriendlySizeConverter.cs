using System;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    class FriendlySizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            long size = System.Convert.ToInt64(value);
            if (size <= 0)
            {
                return "0 B";
            }
            else
            {
                var units = new[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
                int index = 0;
                double cursor = size * 1.0;
                while (cursor >= 1024)
                {
                    index++;
                    cursor /= 1024;
                }
                return cursor.ToString("0.00").Replace(".00", string.Empty) + " " + units[index];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
