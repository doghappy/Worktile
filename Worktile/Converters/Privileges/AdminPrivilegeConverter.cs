using System;
using Windows.UI.Xaml.Data;
using Worktile.Enums.Privileges;

namespace Worktile.Converters.Privileges
{
    class AdminPrivilegeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int permission = (int)value;
            int enumValue = (int)Enum.Parse(typeof(AdminPrivilege), parameter.ToString());
            return (permission & enumValue) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
