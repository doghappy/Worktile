using System;
using Windows.UI.Xaml.Data;
using Worktile.Models;

namespace Worktile.Message.Converters
{
    class IsGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((SessionType)value == SessionType.Session)
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
