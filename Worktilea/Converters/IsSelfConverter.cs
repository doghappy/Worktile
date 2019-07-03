using System;
using Windows.UI.Xaml.Data;
using Worktile.Common;

namespace Worktile.Converters
{
    public class IsSelfConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
            {
                return value.ToString() == DataSource.ApiUserMeData.Me.Uid;
            }
            else
            {
                if (parameter.ToString() == "!")
                {
                    return value.ToString() != DataSource.ApiUserMeData.Me.Uid;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
