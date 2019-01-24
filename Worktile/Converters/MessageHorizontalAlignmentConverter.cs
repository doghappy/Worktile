using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Worktile.Common;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.Converters
{
    class MessageHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString() == DataSource.ApiUserMe.Uid)
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
