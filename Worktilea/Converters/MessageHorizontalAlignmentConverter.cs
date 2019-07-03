using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Worktile.Common;

namespace Worktile.Converters
{
    class MessageHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString() == DataSource.ApiUserMeData.Me.Uid)
            {
                return HorizontalAlignment.Right;
            }
            return HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
