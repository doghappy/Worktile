using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Common;
using Worktile.Main;

namespace Worktile.Converters
{
    public class AvatarSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                string avatar = value.ToString();
                if (avatar != string.Empty && avatar != "default.png")
                {
                    int index = avatar.LastIndexOf('.');
                    string size = "80x80";
                    if (parameter != null)
                    {
                        size = parameter.ToString();
                    }
                    avatar = avatar.Insert(index, '_' + size);
                    //string uri = MainViewModel.Box.AvatarUrl + avatar;
                    string uri = SharedData.Box.AvatarUrl + avatar;
                    var img = new BitmapImage
                    {
                        UriSource = new Uri(uri)
                    };
                    return img;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
