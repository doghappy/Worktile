//using System;
//using Windows.UI.Xaml.Data;

//namespace Worktile.Converters
//{
//    public class DateTimeConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, string language)
//        {
//            if (value == null)
//            {
//                return string.Empty;
//            }
//            else
//            {
//                if (parameter == null)
//                {
//                    return value;
//                }
//                else
//                {
//                    return ((DateTime)value).ToString(parameter.ToString());
//                }
//            }
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, string language)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
