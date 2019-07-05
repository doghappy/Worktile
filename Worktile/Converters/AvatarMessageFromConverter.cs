using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Main;
using Worktile.Message.Models;

namespace Worktile.Converters
{
    public class AvatarMessageFromConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MessageFrom mf)
            {
                int index = mf.Avatar.LastIndexOf('.');
                string url = null;
                if (mf.Type == FromType.Service)
                {
                    url = MainViewModel.Box.ServiceUrl + mf.Avatar;
                }
                else
                {
                    string size = "80x80";
                    if (parameter != null)
                    {
                        size = parameter.ToString();
                    }
                    string avatar = mf.Avatar.Insert(index, '_' + size);
                    url = MainViewModel.Box.AvatarUrl + avatar;
                }
                var img = new BitmapImage
                {
                    UriSource = new Uri(url)
                };
                return img;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
