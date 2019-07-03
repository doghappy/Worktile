using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    public class CountVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = Visibility.Collapsed;
            var data = value as IEnumerable<object>;
            if (data.Count() > 0)
            {
                visibility= Visibility.Visible;
            }
            if (parameter != null && parameter.ToString() == "!")
            {
                if (visibility == Visibility.Visible)
                {
                    visibility = Visibility.Collapsed;
                }
                else if(visibility == Visibility.Collapsed)
                {
                    visibility = Visibility.Visible;
                }
            }
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
