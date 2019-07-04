using System;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Data;

namespace Worktile.Converters
{
    public class AvatarInitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string name = value.ToString();
            if (Regex.IsMatch(name, @"^[\u4e00-\u9fa5]+$") && name.Length > 2)
            {
                return name.Substring(name.Length - 2, 2);
            }
            else if (Regex.IsMatch(name, @"^[a-zA-Z\/\s]+$") && name.IndexOf(' ') > 0)
            {
                var arr = name.Split(' ');
                return (arr[0].First().ToString() + arr[1].First()).ToUpper();
            }
            else if (name.Length > 2)
            {
                return name.Substring(0, 2).ToUpper();
            }
            else
            {
                return name.ToUpper();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
