using System;
using Windows.UI.Xaml.Data;
using Worktile.Common;

namespace Worktile.Converters
{
    public class IsNotSelfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString() != DataSource.ApiUserMeData.Me.Uid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
